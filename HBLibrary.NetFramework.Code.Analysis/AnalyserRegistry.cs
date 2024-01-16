using HBLibrary.NetFramework.Code.Analysis.Analyser;
using HBLibrary.NetFramework.Code.Analysis.Analyser.Results;
using HBLibrary.NetFramework.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public class AnalyserRegistry : IAnalyserRegistry {
        private readonly Dictionary<string, ICodeAnalyser> registeredAnalysers = new Dictionary<string, ICodeAnalyser>();
        public IReadOnlyDictionary<string, ICodeAnalyser> RegisteredAnalyser => registeredAnalysers;
        public AnalysisScope Scope { get; private set; }
        public SemanticModelCache SemanticModelCache { get; private set; }
        public Solution Solution { get; private set; }
        public Project ScopedProject { get; private set; }
        public IImmutableSet<Document> Documents { get; private set; }


        public async Task InitAsync(Project project) {
            Scope = AnalysisScope.Project;
            Solution = project.Solution;
            ScopedProject = project;
            Documents = Solution.Projects.SelectMany(e => e.Documents).ToImmutableHashSet();
            SemanticModelCache = await SemanticModelCache.FromDocumentsAsync(Documents);
        }

        public async Task InitAsync(Solution solution) {
            Scope = AnalysisScope.Solution;
            Solution = solution;
            Documents = Solution.Projects.SelectMany(e => e.Documents).ToImmutableHashSet();
            SemanticModelCache = await SemanticModelCache.FromDocumentsAsync(Documents);
        }

        public static async Task<AnalyserRegistry> FromSolutionAsync(Solution solution) {
            AnalyserRegistry registry = new AnalyserRegistry();
            await registry.InitAsync(solution);
            return registry;
        }

        public static async Task<AnalyserRegistry> FromProjectAsync(Project project) {
            AnalyserRegistry registry = new AnalyserRegistry();
            await registry.InitAsync(project);
            return registry;
        }

        public void Register<TAnalyser>(TAnalyser analyser) where TAnalyser : ICodeAnalyser {
            string typeName = typeof(TAnalyser).FullName;
            if (registeredAnalysers.ContainsKey(typeName))
                throw new AnalyserException($"{typeName} is already registered.");

            registeredAnalysers[typeName] = analyser;
        }

        public TAnalyser Get<TAnalyser>() where TAnalyser : ICodeAnalyser {
            string typeName = typeof(TAnalyser).FullName;
            if (!registeredAnalysers.ContainsKey(typeName))
                throw new AnalyserException($"{typeName} is not registered.");

            return (TAnalyser)registeredAnalysers[typeName];
        }
    }
}

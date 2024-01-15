using HBLibrary.NetFramework.Code.Analysis.Analyser;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public class AnalyserFactory : IAnalyserFactory {
        public AnalyserFactoryScope Scope { get; private set; }
        public SemanticModelCache SemanticModelCache { get; private set; }
        public Solution Solution { get; private set; }
        public Project ScopedProject { get; private set; }
        public IImmutableSet<Document> Documents { get; private set; }

        public async Task Init(Project project) {
            Scope = AnalyserFactoryScope.Project;
            Solution = project.Solution;
            ScopedProject = project;
            Documents = Solution.Projects.SelectMany(e =>  e.Documents).ToImmutableHashSet();
            SemanticModelCache = await SemanticModelCache.FromDocuments(Documents);
        }

        public async Task Init(Solution solution) {
            Scope = AnalyserFactoryScope.Solution;
            Solution = solution;
            Documents = Solution.Projects.SelectMany(e => e.Documents).ToImmutableHashSet();
            SemanticModelCache = await SemanticModelCache.FromDocuments(Documents);
        }

        public static async Task<AnalyserFactory> FromSolution(Solution solution) {
            AnalyserFactory factory = new AnalyserFactory();
            factory.Scope = AnalyserFactoryScope.Solution;
            factory.Solution = solution;
            factory.Documents = solution.Projects.SelectMany(e => e.Documents).ToImmutableHashSet();
            factory.SemanticModelCache = await SemanticModelCache.FromDocuments(factory.Documents);
            return factory;
        }

        public static async Task<AnalyserFactory> FromProject(Project project) {
            AnalyserFactory factory = new AnalyserFactory();
            factory.Scope = AnalyserFactoryScope.Project;
            factory.Solution = project.Solution;
            factory.ScopedProject = project;
            factory.Documents = factory.Solution.Projects.SelectMany(e => e.Documents).ToImmutableHashSet();
            factory.SemanticModelCache = await SemanticModelCache.FromDocuments(factory.Documents);
            return factory;
        }

        public IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser() {
            throw new NotImplementedException();
        }
    }
}

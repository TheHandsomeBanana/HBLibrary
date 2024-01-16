using HBLibrary.NetFramework.Code.Analysis.Analyser;
using HBLibrary.NetFramework.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public class AnalyserFactory : IAnalyserFactory {
        public IAnalyserRegistry Registry { get; }
        public TAnalyser CreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser => Activator.CreateInstance<TAnalyser>();

        public TAnalyser CreateAndRegisterAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser {
            string typeName = typeof(TAnalyser).FullName;
            if (Registry.RegisteredAnalyser.ContainsKey(typeName))
                throw new AnalyserException($"{typeName} already registered.");

            TAnalyser analyser = CreateAnalyser<TAnalyser>();
            Registry.Register(analyser);
            return analyser;
        }

        public IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser() {
            return new ObjectAssignmentAnalyser();
        }
    }
}


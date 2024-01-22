using HBLibrary.NetFramework.Code.Analysis.Analyser;
using HBLibrary.NetFramework.Code.Analysis.Exceptions;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.NetFramework.Code.Analysis {
    public class AnalyserFactory : IAnalyserFactory {
        public IAnalyserRegistry Registry { get; }
        public TAnalyser CreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser => Activator.CreateInstance<TAnalyser>();

        public TAnalyser GetOrCreateAnalyser<TAnalyser>() where TAnalyser : ICodeAnalyser {
            string typeName = typeof(TAnalyser).FullName;
            if (Registry.RegisteredAnalyser.ContainsKey(typeName))
                throw AnalyserException.AnalyserRegistered(typeName);

            TAnalyser analyser = CreateAnalyser<TAnalyser>();
            Registry.Register(analyser);
            return analyser;
        }

        public IObjectAssignmentAnalyser CreateObjectAssignmentAnalyser() {
            return new ObjectAssignmentAnalyser();
        }

        public IObjectAssignmentAnalyser GetOrCreateObjectAssignmentAnalyser() {
            if (Registry.RegisteredAnalyser.ContainsKey(nameof(IObjectAssignmentAnalyser)))
                throw AnalyserException.AnalyserRegistered(nameof(IObjectAssignmentAnalyser));

            ObjectAssignmentAnalyser analyser = new ObjectAssignmentAnalyser();
            Registry.Register(analyser);
            return analyser;
        }
    }
}


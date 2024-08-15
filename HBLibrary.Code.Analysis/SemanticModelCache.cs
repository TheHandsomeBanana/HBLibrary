using Microsoft.CodeAnalysis;

namespace HBLibrary.Code.Analysis;
public class SemanticModelCache {
    private readonly Dictionary<string, SemanticModel> modelCollection = [];

    private readonly List<string> duplicates = [];
    public IReadOnlyCollection<string> Duplicates => duplicates;

    public static async Task<SemanticModelCache> FromSolutionAsync(Solution solution, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(solution.Projects.SelectMany(e => e.Documents), cancellationToken);
        return modelCache;
    }

    public static async Task<SemanticModelCache> FromProjectAsync(Project project, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(project.Documents, cancellationToken);
        return modelCache;
    }

    public static async Task<SemanticModelCache> FromDocumentsAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
        SemanticModelCache modelCache = new SemanticModelCache();
        await modelCache.InitAsync(documents, cancellationToken);
        return modelCache;
    }

    private async Task InitAsync(IEnumerable<Document> documents, CancellationToken cancellationToken = default) {
        foreach (var document in documents) {
            if (document.FilePath == null || !document.SupportsSemanticModel)
                continue;

            if (modelCollection.ContainsKey(document.FilePath)) {
                duplicates.Add(document.FilePath);
                continue;
            }

            SemanticModel loadedModel = (await document.GetSemanticModelAsync(cancellationToken))!;
            modelCollection.Add(document.FilePath, loadedModel);
        }
    }
}

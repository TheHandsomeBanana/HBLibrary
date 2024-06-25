using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBLibrary.Wpf.Services.FrameNavigationService;
public class FrameNavigationBuilder : IFrameNavigationBuilder {
    private readonly Dictionary<string, FrameNavigationModel> result = [];
    
    public IFrameNavigationBuilder Add(string frameKey, Func<IFrameNavigationModelBuilder, FrameNavigationModel> frameNavigationModelBuilder) {
        result.Add(frameKey, frameNavigationModelBuilder(new FrameNavigationModelBuilder()));
        return this;
    }

    public Dictionary<string, FrameNavigationModel> Build() {
        return result;
    }
}

public class FrameNavigationModelBuilder : IFrameNavigationModelBuilder {
    private readonly Dictionary<Uri, Type> viewModelMapping = [];
    private string basePath = "";
    public IFrameNavigationModelBuilder Add(Uri uri, Type type) {
        viewModelMapping.Add(uri, type);
        return this;
    }

    public IFrameNavigationModelBuilder AddBasePath(string basePath) {
        this.basePath = basePath;
        return this;
    }

    public FrameNavigationModel Build() {
        return new FrameNavigationModel(basePath, viewModelMapping);
    }
}

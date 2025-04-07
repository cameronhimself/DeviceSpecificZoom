using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

namespace DeviceSpecificZoom {
  public sealed class DeviceConfig {
    public float zoomLevel { get; set; } = 1;
    public float uiScale { get; set; } = 1;
  }

  internal sealed class ModEntry: Mod {
    private DeviceConfig config = new();
    private bool hasChanges = false;
    private const string CONFIG_FILENAME = "device-config";

    public override void Entry(IModHelper helper) {
      helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
      helper.Events.GameLoop.SaveLoaded += this.OnSaveLoaded;
    }

    private void OnSaveLoaded(object? sender, SaveLoadedEventArgs e) {
      this.config = this.Helper.Data.ReadGlobalData<DeviceConfig>(CONFIG_FILENAME) ?? new DeviceConfig() {
        uiScale = Game1.options.uiScale,
        zoomLevel = Game1.options.zoomLevel,
      };
      Game1.options.desiredUIScale = this.config.uiScale;
      Game1.options.desiredBaseZoomLevel = this.config.zoomLevel;
    }

    private void OnUpdateTicked(object? sender, EventArgs e) {
      if (this.hasChanges) {
        if (Game1.activeClickableMenu == null) {
          this.Helper.Data.WriteGlobalData(CONFIG_FILENAME, this.config);
          this.hasChanges = false;
        }
      }

      if (Context.IsWorldReady) {
        if (this.config.uiScale != Game1.options.uiScale) {
          this.config.uiScale = Game1.options.uiScale;
          this.hasChanges = true;
        }
        if (this.config.zoomLevel != Game1.options.zoomLevel) {
          this.config.zoomLevel = Game1.options.zoomLevel;
          this.hasChanges = true;
        }
      }
    }
  }
}

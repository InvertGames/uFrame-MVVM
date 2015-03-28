using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uFrame.Graphs
{
    public static class uFrameFormats
    {
        public const string VIEW_MODEL_FORMAT = "{0}ViewModel";
        public const string VIEW_FORMAT = "{0}View";
        public const string VIEW_COMPONENT_FORMAT = "{0}ViewComponent";
        public const string SCENE_MANAGER_FORMAT = "{0}";
        public const string SCENE_MANAGER_SETTINGS_FORMAT = "{0}Settings";
        public const string CONTROLLER_FORMAT = "{0}Controller";

        public const string SERVICE_FORMAT = "{0}";

        public const string SUBSCRIBABLE_FIELD_FORMAT = "_{0}Property";
        public const string SUBSCRIBABLE_PROPERTY_FORMAT = "{0}Property";
        public const string PROPERTY_FORMAT = "{0}";
        public const string FIELD_FORMAT = "_{0}";

        public static string AsService(this string s)
        {
            return string.Format(SERVICE_FORMAT, s);
        }

        public static string AsView(this string s)
        {
            return string.Format(VIEW_FORMAT, s);
        }
        public static string AsViewComponent(this string s)
        {
            return string.Format(VIEW_COMPONENT_FORMAT, s);
        }
        public static string AsViewModel(this string s)
        {
            return string.Format(VIEW_MODEL_FORMAT, s);
        }
        public static string AsController(this string s)
        {
            return string.Format(CONTROLLER_FORMAT, s);
        }
        public static string AsSceneManager(this string s)
        {
            return string.Format(SCENE_MANAGER_FORMAT, s);
        }
        public static string AsSceneManagerSettings(this string s)
        {
            return string.Format(SCENE_MANAGER_SETTINGS_FORMAT, s);
        }
        public static string AsSubscribableProperty(this string s)
        {
            return string.Format(SUBSCRIBABLE_PROPERTY_FORMAT, s);
        }
        public static string AsSubscribableField(this string s)
        {
            return string.Format(SUBSCRIBABLE_FIELD_FORMAT, s);
        }
        public static string AsField(this string s)
        {
            return string.Format(FIELD_FORMAT, s);
        }
        public static string AsProperty(this string s)
        {
            return string.Format(PROPERTY_FORMAT, s);
        }
        
    }
}

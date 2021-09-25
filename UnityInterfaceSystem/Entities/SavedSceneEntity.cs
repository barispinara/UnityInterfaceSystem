using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityInterfaceSystem.Entities
{
    public class SavedSceneEntity
    {
        public string Scene_Name { get; set; }

        public List<SceneTaskEntity> task_list { get; set; }

        public SavedSceneEntity(string sceneName, List<SceneTaskEntity> taskList)
        {
            this.Scene_Name = sceneName;
            this.task_list = taskList;
        }
    }
}

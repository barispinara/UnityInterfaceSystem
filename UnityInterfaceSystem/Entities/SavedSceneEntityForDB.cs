using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityInterfaceSystem.Entities
{
    public class SavedSceneEntityForDB
    {
        public string Scene_Name { get; set; }

        public string Task_Name { get; set; }

        public string Subtask_Name { get; set; }

        public string ID { get; set; }

        public string Model_Name { get; set; }

        public string Piece_Name { get; set; }

        public SavedSceneEntityForDB(string sceneName, string taskName, string id)
        {
            this.Scene_Name = sceneName;
            this.Task_Name = taskName;
            this.ID = id;
        }

        public SavedSceneEntityForDB(string sceneName, string taskName, string subtaskName, string id, string modelName, string pieceName)
        {
            this.Scene_Name = sceneName;
            this.Task_Name = taskName;
            this.Subtask_Name = subtaskName;
            this.ID = id;
            this.Model_Name = modelName;
            this.Piece_Name = pieceName;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System.Linq;

namespace GameData
{
    [CreateAssetMenu(fileName = "DialogDataSO", menuName = "ScriptableObject/Data/DialogSO")]
    public class DialogDataSO : ScriptableData
    {
        [Header("Npc")]
        [SerializeField] private string _npcName;
        [Header("DialogText")]
        [SerializeField, TextArea] private string _mainDialogText;
        [SerializeField, TextArea] private string _loopDialogText;
        [SerializeField, TextArea] private string _endDialogText;

        public string NpcName => _npcName;
        public override ScriptableDataKey Key => _key;

        public List<string> GetMainDialogList()
        {
            var list = ParseDialog(_mainDialogText);
            return list;
        }

        public List<string> GetLoopDialogList()
        {
            var list = ParseDialog(_loopDialogText);
            return list;
        }

        public List<string> GetEndDialogList()
        {
            var list = ParseDialog(_endDialogText);
            return list;
        }

        private List<string> ParseDialog(string dialogText)
        {
            var splitArray = dialogText.Split("{E}");
            var dialogList = splitArray.ToList();
            return dialogList;
        }
    }
}


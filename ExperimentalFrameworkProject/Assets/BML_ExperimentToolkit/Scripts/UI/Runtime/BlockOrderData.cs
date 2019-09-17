using BML_ExperimentToolkit.Scripts.ExperimentParts;

namespace BML_ExperimentToolkit.Scripts.UI.Editor {
    public class BlockOrderData {
        
        
        public readonly bool   SelectionRequired      = false;
        public readonly int    DefaultBlockOrderIndex = 0;
        public readonly string BlockOrderText;
        
        public BlockOrderData(ExperimentDesign experimentDesign) {
            
            if (experimentDesign.NumberOfBlocks > 1) {
                BlockOrderText = "Select a Block order";
                SelectionRequired = true;
            }
            else if (experimentDesign.NumberOfBlocks == 1) {
                BlockOrderText = "Only one Block value";
                DefaultBlockOrderIndex = 0;
            }
            else {
                BlockOrderText = "No Block variables";
                DefaultBlockOrderIndex = 0;
            }
        }

    }
}
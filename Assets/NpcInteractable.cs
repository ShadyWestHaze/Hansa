using UnityEngine;

namespace KMVGS.FinalCharacterController
{
    public class NPCInteractable : Interactable
    {
        [TextArea] [SerializeField] private string dialogueText;
        
        public override void OnInteract(PlayerInteractor interactor)
        {
            base.OnInteract(interactor);
            
            // NPC-specific interaction
            if (!interactor.IsHoldingItem())
            {
                Debug.Log(dialogueText);
                // You would replace this with your actual dialogue system
            }
            else
            {
                Debug.Log("Maybe you want to give the NPC what you're holding?");
                // Handle giving items to NPCs
            }
        }
    }
}
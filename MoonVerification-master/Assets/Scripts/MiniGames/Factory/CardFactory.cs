using Core.Customs;
using UnityEngine;


namespace Core
{
    public sealed class CardFactory
    {
        public Card CreateCard(Vector3 cardPosition, Quaternion cardRotation)
        {
            var cardBehaviour = CustomResources.Load<CardBehaviour>
                            (AssetsPathGameObject.GameObjects[GameObjectType.Card]);
            var gameObject = Object.Instantiate(cardBehaviour, cardPosition, cardRotation).gameObject;

            var result = new Card(gameObject);
            return result;
        }
        
    }
}

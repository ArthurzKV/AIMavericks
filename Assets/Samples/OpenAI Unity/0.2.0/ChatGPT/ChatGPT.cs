using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace OpenAI
{
    public class ChatGPT : MonoBehaviour
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private ScrollRect scroll;

        [SerializeField] private RectTransform sent;
        [SerializeField] private RectTransform received;
        [SerializeField] private GameObject canvasObject; // Referencia al canvas

        public GameObject collisionObject; // Objeto con el que debe colisionar

        private float height;
        private OpenAIApi openai = new OpenAIApi(); // Asegúrate de inicializar correctamente

        private List<ChatMessage> messages = new List<ChatMessage>();
        public string prompt; // Variable pública para el prompt
        public bool sarcastic = false;
        public bool fantasy = false;

        public GameObject collisionObject1;
        public GameObject collisionObject2;

        private void AppendMessage(ChatMessage message)
        {
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

            var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
            item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
            item.anchoredPosition = new Vector2(0, -height);
            LayoutRebuilder.ForceRebuildLayoutImmediate(item);
            height += item.sizeDelta.y;
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            scroll.verticalNormalizedPosition = 0;

            StartCoroutine(RemoveAfterSeconds(10, item.gameObject, canvasObject));
        }

        IEnumerator RemoveAfterSeconds(int seconds, GameObject obj, GameObject canvas)
        {
            yield return new WaitForSeconds(seconds);

            RectTransform objRect = obj.GetComponent<RectTransform>();
            height -= objRect.sizeDelta.y;

            Destroy(obj);

            yield return null;

            UpdateMessagePositions();

            LayoutRebuilder.ForceRebuildLayoutImmediate(scroll.content);

            if (scroll.content.childCount == 0)
            {
                canvas.SetActive(false);
            }
        }

        void UpdateMessagePositions()
        {
            float currentHeight = 0;
            foreach (RectTransform child in scroll.content)
            {
                child.anchoredPosition = new Vector2(0, -currentHeight);
                LayoutRebuilder.ForceRebuildLayoutImmediate(child);
                currentHeight += child.sizeDelta.y;
            }
            scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, currentHeight);
        }

        public async void SendReply()
        {
            var modelId = GetModelId(); // Obtiene el ID del modelo basado en los flags

            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = prompt
            };

            if (messages.Count == 0) newMessage.Content = prompt;

            messages.Add(newMessage);

            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = modelId,
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                AppendMessage(message);
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
        }

        private string GetModelId()
        {
            // Retorna el ID del modelo sin cambiar los estados aquí
            if (sarcastic)
            {
                return "ft:gpt-3.5-turbo-1106:personal:finalbot:9YKGGyqH";
            }
            else
            {
                return "ft:gpt-3.5-turbo-0125:personal:sarcasticbot:9XxwFVbe";
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Collision with Object 1 Detected");
            if (other.CompareTag("Player")) // Verifica si el objeto colisionado tiene el tag "Player"
            {
                if (other.gameObject == collisionObject1)
                {
                    // Alternar a modo sarcástico
                    sarcastic = true;
                    fantasy = false;
                    Debug.Log("Collision with Object 1 Detected");
                }
                else if (other.gameObject == collisionObject2)
                {
                    // Alternar a modo fantasía
                    sarcastic = false;
                    fantasy = true;
                    Debug.Log("Collision with Object 2 Detected");
                }

                // Llama a SendReply solo si el objeto colisionado tiene el tag "Player"
                SendReply();
            }
        }
    }
}
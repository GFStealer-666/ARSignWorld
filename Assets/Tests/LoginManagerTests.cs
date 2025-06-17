// using NUnit.Framework;
// using UnityEngine;
// using UnityEngine.TestTools;
// using System.Collections;
// using UnityEngine.Networking;
// using System.Text;

// [TestFixture]
// public class LoginManagerTests
// {
//     //private LoginManager loginManager;
//     private GameObject gameObject;

//     [SetUp]
//     public void Setup()
//     {
//         gameObject = new GameObject();
//         //loginManager = gameObject.AddComponent<LoginManager>();
//         // Setup mock UI components
//         //loginManager.usernameInput = new MockInputField { text = "" };
//         //loginManager.passwordInput = new MockInputField { text = "" };
//     }

//     [TearDown]
//     public void Teardown()
//     {
//         Object.DestroyImmediate(gameObject);
//     }

//     [Test]
//     public void AttemptLogin_EmptyCredentials_ShowsNotification()
//     {
//         // Arrange
//         loginManager.usernameInput.text = "";
//         loginManager.passwordInput.text = "";

//         // Act
//         loginManager.AttemptLogin();

//         // Assert
//         // Verify notification was shown (you'll need to add a way to check this)
//         Assert.That(loginManager.IsNotificationShown, Is.True);
//     }

//     [UnityTest]
//     public IEnumerator LoginRequest_ValidCredentials_LoginSuccessful()
//     {
//         // Arrange
//         string email = "test@example.com";
//         string password = "validPassword";

//         // Act
//         yield return loginManager.LoginRequest(email, password);

//         // Assert
//         Assert.That(loginManager.IsLoggedIn, Is.True);
//         Assert.That(loginManager._SuccessfulLoginUI.activeSelf, Is.True);
//     }

//     [UnityTest]
//     public IEnumerator LoginRequest_InvalidCredentials_ShowsError()
//     {
//         // Arrange
//         string email = "invalid@example.com";
//         string password = "wrongPassword";

//         // Act
//         yield return loginManager.LoginRequest(email, password);

//         // Assert
//         Assert.That(loginManager.IsLoggedIn, Is.False);
//         // Check error handling
//         Assert.That(loginManager.HasLoginError, Is.True);
//     }

// //     public void AttemptLogin()
// // {
// //     string email = usernameInput.text;
// //     string password = passwordInput.text;

// //     if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
// //     {
// //         ShowNotification(false);
// //         return;
// //     }
// //     Debug.Log("Attempting to login with email: " + email);
// //     StartCoroutine(LoginRequest(email, password));
// // }

// // IEnumerator LoginRequest(string email, string password)
// // {
// //     string jsonData = $"{{"email":"{email}","password":"{password}"}}";
// //     byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

// //     using (UnityWebRequest request = new UnityWebRequest(loginAPIUrl, "POST"))
// //     {
// //         request.uploadHandler = new UploadHandlerRaw(jsonToSend);
// //         request.downloadHandler = new DownloadHandlerBuffer();
// //         request.SetRequestHeader("Content-Type", "application/json");

// //         yield return request.SendWebRequest();

// //         string responseText = request.downloadHandler.text;
// //         Debug.Log("Response: " + responseText);

// //         if (request.result == UnityWebRequest.Result.Success)
// //         {
// //             _loginResponse = JsonUtility.FromJson<LoginResponse>(responseText);

// //             if (_loginResponse.status)
// //             {
// //                 Debug.Log("Login Successful!");
// //                 _emailNotification.SetActive(false);
// //                 _passwordNotification.SetActive(false);
// //                 _SuccessfulLoginUI.SetActive(true);
// //                 StartCoroutine(LoadNextScene()); // Load next scene
// //             }
// //             else
// //             {
// //                 Debug.LogError("Login failed.");
// //                 HandleLoginError(responseText);
// //             }
// //         }
// //         else
// //         {
// //             Debug.LogError("Request Failed: " + request.error);
// //             HandleLoginError(responseText);
// //         }
// //     }
// // }
// }

// // Mock classes for testing
// public class MockInputField
// {
//     public string text { get; set; }
// }

// // Mock web request for testing
// public class MockUnityWebRequest : UnityWebRequest
// {
//     public bool shouldSucceed;
//     public string mockResponse;

//     public new UnityWebRequest.Result result 
//     { 
//         get => shouldSucceed ? UnityWebRequest.Result.Success : UnityWebRequest.Result.ConnectionError;
//     }
// }
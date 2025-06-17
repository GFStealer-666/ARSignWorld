// #if UNITY_EDITOR
// using UnityEditor;
// using UnityEngine;
// using System.IO;
// using System.Collections.Generic;

// public class QuizSOGenerator
// {
//     // [MenuItem("Tools/Generate ASD")]
//      public static void GenerateQuiz()
//     {
//         string folder = "Assets/Quiz/SolarSystem";
//         if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);

//         string[] questions = {
//             "ระบบสุริยะจักรวาลมีศูนย์กลางคืออะไร?",
//             "ระบบสุริยะมีดาวเคราะห์หลักทั้งหมดกี่ดวง?",
//             "ดาวเคราะห์ในระบบสุริยะโคจรรอบอะไร?",
//             "ดาวเคราะห์ดวงใดอยู่ใกล้ดวงอาทิตย์มากที่สุด?",
//             "ดาวเคราะห์ดวงใดมีวงแหวนขนาดใหญ่และมองเห็นได้ชัดเจน?",
//             "ดาวเคราะห์ที่มนุษย์อาศัยอยู่คือดวงใด?",
//             "ดาวเคราะห์ที่มีลมแรงที่สุดในระบบสุริยะคือดวงใด?",
//             "ดาวเคราะห์ใดที่หมุนเอียงมากจนเหมือนกลิ้งไปในอวกาศ?",
//             "วัตถุใดในระบบสุริยะที่มีหางและเคลื่อนที่เป็นวงรี?",
//             "วัตถุในระบบสุริยะที่อาจตกถึงพื้นโลกและกลายเป็นอุกกาบาตคืออะไร?"
//         };

//         string[][] choices = {
//             new string[] { "ก. โลก", "ข. ดวงจันทร์", "ค. ดวงอาทิตย์", "ง. ดาวเสาร์" },
//             new string[] { "ก. 5 ดวง", "ข. 8 ดวง", "ค. 9 ดวง", "ง. 10 ดวง" },
//             new string[] { "ก. ดวงจันทร์", "ข. ดวงอาทิตย์", "ค. โลก", "ง. ฝุ่นในอวกาศ" },
//             new string[] { "ก. โลก", "ข. ดาวอังคาร", "ค. ดาวพุธ", "ง. ดาวเนปจูน" },
//             new string[] { "ก. ดาวพฤหัสบดี", "ข. ดาวยูเรนัส", "ค. ดาวเนปจูน", "ง. ดาวเสาร์" },
//             new string[] { "ก. ดาวศุกร์", "ข. ดาวโลก", "ค. ดาวยูเรนัส", "ง. ดาวพลูโต" },
//             new string[] { "ก. ดาวอังคาร", "ข. ดาวพฤหัสบดี", "ค. ดาวเนปจูน", "ง. ดาวพุธ" },
//             new string[] { "ก. ดาวอังคาร", "ข. ดาวเสาร์", "ค. ดาวยูเรนัส", "ง. ดาวศุกร์" },
//             new string[] { "ก. ดาวเทียม", "ข. ดาวตก", "ค. ดาวหาง", "ง. ดวงจันทร์" },
//             new string[] { "ก. ฝุ่นดาวเคราะห์", "ข. ดาวเทียม", "ค. เศษแก๊ส", "ง. อุกกาบาตเล็ก" }
//         };

//         int[] correctAnswerIndex = { 2, 1, 1, 2, 3, 1, 2, 2, 2, 3 };

//         for (int i = 0; i < questions.Length; i++)
//         {
//             QuizSO quiz = ScriptableObject.CreateInstance<QuizSO>();
//             quiz.questionText = questions[i];
//             quiz.choices = choices[i];
//             quiz.correctAnswerIndex = correctAnswerIndex[i];

//             string assetName = $"ระบบสุริยะจักรวาล ข้อ {i + 1}.asset";
//             string assetPath = Path.Combine(folder, assetName);
//             AssetDatabase.CreateAsset(quiz, assetPath);
//         }

//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();

//         Debug.Log("✅ QuizSO for ชั้นบรรยากาศโลก created successfully!");
//     }
// }
// #endif

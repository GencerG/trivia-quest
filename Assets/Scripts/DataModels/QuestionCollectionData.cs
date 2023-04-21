using System;
using System.Collections.Generic;

[Serializable]
public class QuestionCollectionData
{
    public readonly List<QuestionData> questions;
}

[Serializable]
public class QuestionData
{
    public string category;
    public string question;
    public List<string> choices;
    public string answer;
}

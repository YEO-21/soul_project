using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(
    fileName = "NpcInfo",
    menuName = "ScriptableObject/NpcInfo",
    order = int.MinValue)]
public sealed class NpcInfoScriptableObject : ScriptableObject
{
    public List<NpcInfo> m_NpcInfos;

    public NpcInfo GetNpcInfoFromCode(string code)
        => m_NpcInfos.Find(elem => elem.m_Code == code);

}

[System.Serializable]
public class NpcInfo
{
    [Header("# NpcCode")]
    public string m_Code;

    [Header("# Npc �̸�")]
    public string m_Name;


    [Header("# Npc Ÿ��")]
    public NpcType m_Type;

    [Header("# �⺻ ��ȭ")]
    [Multiline]
    public string m_DefaultDialog;

    [Header("# Params")]
    public List<string> m_StringParams;



}


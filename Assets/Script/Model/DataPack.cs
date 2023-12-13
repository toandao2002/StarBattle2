using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataPack
{
    public List<Pack> pack;
    public  DataPack()
    {
        if (pack == null)
        {
            pack = new List<Pack>()
            {
                new Pack(((TypeGame) 0)),
                new Pack(((TypeGame) 1)),
                new Pack(((TypeGame) 2)),
                new Pack(((TypeGame) 3))
            };
        }
    }
    public bool CheckPackBeBought(TypeGame typeGame, int idPack)
    {
        
        return pack[(int)typeGame].CheckPackBeBought(idPack);
        
    }
    public void BuyBack(TypeGame typeGame, int idPack)
    {
        pack[(int)typeGame].BuyPack(idPack);
        DataGame.SetDataJson(DataGame.Datapack, Util.ConvertObjectToString(this));
        DataGame.Save();
    }
    public bool CheckLevelHasBeenBought(int level, TypeGame typeGame, LevelCommon levelCommon)
    {
        return pack[(int)typeGame].CheckLevelHasBeenBought(level, levelCommon);
    }
}
[Serializable]
public class Pack
{
    public TypeGame typeGame;
    public List<int> idBoughtPack;
    public Pack(TypeGame  typegame)
    {
        this.typeGame = typegame;
        idBoughtPack = new List<int>();
    }
    public bool CheckPackBeBought(int idPack) {
        return idBoughtPack.Contains(idPack);
    }
    public void BuyPack(int idPack)
    {
        idBoughtPack.Add(idPack);
    }
    public bool CheckLevelHasBeenBought(int idLevel, LevelCommon levelCommon)
    {
        if (idBoughtPack.Count == 0) {
            return levelCommon.GetSubLevel(typeGame)[0].GetAmountLevel()> idLevel;
             
        }
        int idPackLast = idBoughtPack[idBoughtPack.Count - 1]-1;
        return levelCommon.GetSubLevel(typeGame)[idPackLast].GetAmountLevel() > idLevel;
         
    }
}

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PunPlayerScores.cs" company="Exit Games GmbH">
//   Part of: Photon Unity Utilities,
// </copyright>
// <summary>
//  Scoring system for PhotonPlayer
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.UtilityScripts
{
    /// <summary>
    /// Scoring system for PhotonPlayer
    /// </summary>
    public class PunPlayerScores : MonoBehaviour
    {
        public const string PlayerScoreProp = "score";
    }


    public class PunPlayerWin : MonoBehaviour
    {
        public const string PlayerWin = "win";
    }

    public class PunPlayerDone : MonoBehaviour
    {
        public const string PlayerDone = "Done";
    }


    public static class WinExtensions
    {

        public static void SetWin(this Player player, bool onoff)
        {
            Hashtable win = new Hashtable();
            win[PunPlayerWin.PlayerWin] = onoff;

            player.SetCustomProperties(win);
        }

        public static bool GetWin(this Player player)
        {
            object win;
            if (player.CustomProperties.TryGetValue(PunPlayerWin.PlayerWin, out win))
            {
                return (bool)win;
            }

            return false;
        }
    }


    public static class DoneExtension
    {

        public static void SetDone(this Player player, bool onoff)
        {
            Hashtable done = new Hashtable();
            done[PunPlayerDone.PlayerDone] = onoff;

            player.SetCustomProperties(done);
        }

        public static bool GetDone(this Player player)
        {
            object done;
            if (player.CustomProperties.TryGetValue(PunPlayerDone.PlayerDone, out done))
            {
                return (bool)done;
            }

            return false;
        }
    }





    public static class ScoreExtensions
    {
        public static void SetScore(this Player player, int newScore)
        {
            Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
            score[PunPlayerScores.PlayerScoreProp] = newScore;

            player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
        }

        public static void AddScore(this Player player, int scoreToAddToCurrent)
        {
            int current = player.GetScore();
            current = current + scoreToAddToCurrent;

            Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
            score[PunPlayerScores.PlayerScoreProp] = current;

            player.SetCustomProperties(score);  // this locally sets the score and will sync it in-game asap.
        }

        public static int GetScore(this Player player)
        {
            object score;
            if (player.CustomProperties.TryGetValue(PunPlayerScores.PlayerScoreProp, out score))
            {
                return (int)score;
            }

            return 0;
        }
    }
}
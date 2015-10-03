using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace VK
{
    public class VKClient
    {
        public static String IDUser(String url)
        {
            String str1 = "";
            String str2 = "";
            for (int i = url.Length; i > 0; i--)
            {
                if (url[i - 1] != '/')
                {
                    str1 += url[i - 1];
                }
                else break;
            }
            for (int j = str1.Length; j > 0; j--)
            {
                str2 = str2 + str1[j - 1];
            }
            return str2;
        }

        public static String GetId(String url)
        {
            String id = "";
            StringBuilder param = new StringBuilder("uids=");
            param.Append(IDUser(url));
            String html = VK.HTTPMaster.PageData("getProfiles", param.ToString());
            id = VK.JSONMaster.GetID_Of_JSON(html);
            return id;
        }

        public static List<String> FriendsList(String id)
        {
            List<String> ListFR = new List<String>();
            StringBuilder param = new StringBuilder("user_id=");
            param.Append(id);
            String html = VK.HTTPMaster.PageData("friends.get", param.ToString());
            ListFR = VK.JSONMaster.GetFriendsList_Of_JSON(html);
            return ListFR;
        }

        public static List<String> GroupsList(String id)
        {
            List<String> ListGR = new List<String>();
            StringBuilder param = new StringBuilder("user_id=");
            param.Append(id);
            String token = @"&access_token=ed1167c29581459e8e766bba8e9acba48db9915d2355e449bef98f81ad6cae363eb982ab186815d4388b9";
            // https://oauth.vk.com/authorize?client_id=5077174&display=popup&redirect_uri=http://vk.com&scope=friends,audio,groups&response_type=token&v=5.37
            String html = VK.HTTPMaster.PageData("groups.get", param.ToString() + token + "&v=5.37");
            ListGR = VK.JSONMaster.GetGroupsList_Of_JSON(html);
            return ListGR;
        }

        private static Dictionary<String, List<String>> GroupsOfFriends(List<String> ListFR)
        {
            Dictionary<String, List<String>> ListGR = new Dictionary<String, List<String>>();
            for (int i = 0; i < ListFR.Count(); i++)
            {
                Thread.Sleep(300);
                ListGR[ListFR[i]] = GroupsList(ListFR[i]);
                Console.Write(".");
            }
            return ListGR;
        }

        public static void Top(String url, int quantity)
        {
            VK.StatisticsMaster.SortGroups(VK.StatisticsMaster.RepeatGroups(GroupsOfFriends(FriendsList(GetId(url)))), quantity);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace TimeAtten.Services.Services
{
    public class random
    {
        public static string CleanGUID()
        {
            return System.Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(10, 99));
            builder.Append(RandomString(2, false));

            return builder.ToString();
        }

        public static string CreateSalt()
        {
            return System.Guid.NewGuid().ToString() + DateTime.Now.ToString();
        }

        private static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random((int)DateTime.Now.Ticks);
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
            {
                return builder.ToString().ToLower();
            }
            return builder.ToString();
        }

        private static int RandomNumber(int min, int max)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            return random.Next(min, max);
        }
    }
}
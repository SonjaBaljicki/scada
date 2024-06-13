using ScadaCore.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScadaCore.processing
{
    public static class UserProcessing
    {

        public static bool AddUser(User user)
        {
            using (var db = new DatabaseContext())
            {
                try
                {
                    db.users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            return true;

        }
        public static List<User> GetAllUsers()
        {
            using (var db = new DatabaseContext())
            {
                return db.users.ToList();
                
            }

        }
    }
}
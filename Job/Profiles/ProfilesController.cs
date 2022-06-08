﻿using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Interfaces;

namespace Job.Profiles
{
    public static class ProfilesController
    {
        private const string ProfileFileName = "ProfileSettings.xml";
        private static string _profilesFolder;
        private static readonly List<Profile> Profiles = new List<Profile>();

        public static Profile CurrentProfile;
        //load Profiles
        //save Profiles
        //apply Profile

        public static void LoadProfiles(string profilesFolder)
        {
            _profilesFolder = profilesFolder;

            foreach (var dir in Directory.GetDirectories(_profilesFolder))
            {
                var dirName = Path.GetFileName(dir);

                if (!dirName.StartsWith("-"))
                {
                    var profileFile = Path.Combine(_profilesFolder,dirName, ProfileFileName);

                    var profileSetting = Commons.DeserializeXML<ProfileSettings>(profileFile);

                    var profile = new Profile { Settings = profileSetting, ProfilePath = dir};
                    Profiles.Add(profile);
                }

               
            }

        }

        public static Profile[] GetProfiles()
        {
            return Profiles.ToArray();
        }

        public static Profile[] GetProfiles(string path)
        {
            LoadProfiles(path);
            return GetProfiles();
        }

        public static Profile AddProfile()
        {

            using (var f = new CustomForms.FormEnterText())
            {
                f.Text = "Назва профілю";
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var profile = new Profile();
                    profile.Settings.ProfileName = f.SelectedText;
                    profile.InitProfilePath(_profilesFolder);
                    //todo:check for duplicate name profile

                    profile.InitProfile();

                    Profiles.Add(profile);


                    return profile;
                }
            }

            return null;

        }

        public static void Save()
        {
            foreach (Profile profile in Profiles)
            {
                var userDir = profile.ProfilePath;

                Directory.CreateDirectory(userDir);

                var profileFile = Path.Combine(userDir, ProfileFileName);

                Commons.SerializeXML((ProfileSettings)profile.Settings,profileFile);

            }
        }

        public static void Save(IUserProfile profile)
        {
            var userDir = profile.ProfilePath;

            Directory.CreateDirectory(userDir);

            var profileFile = Path.Combine(userDir, ProfileFileName);

            Commons.SerializeXML((ProfileSettings)profile.Settings,profileFile);
        }

        public static void RemoveProfile(Profile profile)
        {

            Profiles.Remove(profile);

            var profileDir = profile.ProfilePath;

            DeleteDirectory(profileDir);
        }


        static void DeleteDirectory(string targetDir)
        {
            string[] files = Directory.GetFiles(targetDir);
            string[] dirs = Directory.GetDirectories(targetDir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(targetDir, false);
        }

        public static void CloseProgram()
        {
            Profiles.ForEach(x=>x.Exit());
        }
    }
}

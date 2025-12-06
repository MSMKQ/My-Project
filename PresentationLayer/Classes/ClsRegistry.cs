using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer.Classes
{
    public class ClsRegistry
    {
        private static string _KeyName = @"HKEY_CURRENT_USER\Software\FirstAppTest";

        public static object GetValue(string ValueName)
        {
            object result = null;

            try
            {
                result = Registry.GetValue(_KeyName, ValueName, string.Empty);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}.", "Get Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            return result;
        }

        public static bool SetValue<T>(string ValueName, T Value, RegistryValueKind Kind = RegistryValueKind.String)
        {
            try
            {
                Registry.SetValue(_KeyName, ValueName, Value, Kind);
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}.", "Set Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

    }
}

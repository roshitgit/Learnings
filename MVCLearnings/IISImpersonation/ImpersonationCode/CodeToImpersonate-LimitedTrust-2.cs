using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Impersonator
{
    public class Impersonation
    {
        # region Members

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        public const int LOGON32_PROVIDER_DEFAULT = 0;
        private static string _sDocSRVUId = ConfigurationManager.AppSettings["impersonation_uid"];
        private static string _sDDocDomain = ConfigurationManager.AppSettings["impersonation_domain"];
        private static string _sDocSRVPswd = ConfigurationManager.AppSettings["impersonation_pwd"];

        WindowsImpersonationContext impersonationContext;

        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(String lpszUserName, String lpszDomain,
                                            String lpszPassword, int dwLogonType,
                                            int dwLogonProvider, ref IntPtr phToken);
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
                                                int impersonationLevel,
                                                ref IntPtr hNewToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);

        # endregion Members

        # region  Public Properties

        public static string SrvUID
        {
            get { return _sDocSRVUId; }
        }

        public static string SrvPWSD
        {
            get { return _sDocSRVPswd; }
        }

        public static string DocDomain
        {
            get { return _sDDocDomain; }
        }

        # endregion  Public Properties

        # region Public Methods

        /// <summary>
        /// Impersonatin g with specified user credential
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domain"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //public bool impersonateValidUser(String userName, String domain, String password)
        public bool impersonateValidUser()
        {
            try
            {
                WindowsIdentity tempWindowsIdentity;
                IntPtr token = IntPtr.Zero;
                IntPtr tokenDuplicate = IntPtr.Zero;

                if (RevertToSelf())
                {
                    if (LogonUserA(_sDocSRVUId, _sDDocDomain, _sDocSRVPswd, LOGON32_LOGON_INTERACTIVE,
                                   LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                            impersonationContext = tempWindowsIdentity.Impersonate();
                            if (impersonationContext != null)
                            {
                                CloseHandle(token);
                                CloseHandle(tokenDuplicate);
                                return true;
                            }
                        }
                    }
                }
                if (token != IntPtr.Zero)
                    CloseHandle(token);
                if (tokenDuplicate != IntPtr.Zero)
                    CloseHandle(tokenDuplicate);

            }
            catch (System.Exception exp)
            {
            }
            return false;
        }

        /// <summary>
        /// Reverse back to default ASP.NET user account
        /// </summary>
        public void undoImpersonation()
        {
            try
            {
                impersonationContext.Undo();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (System.Exception exp)
            {
            }
        }

        /// <summary>
        /// Reverse back to default ASP.NET user account when given specific path
        /// </summary>
        public void undoImpersonation(string sPath)
        {
            try
            {
                impersonationContext.Undo();
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (System.Exception exp)
            {
            }
        }

        # endregion Public Methods
    }
}

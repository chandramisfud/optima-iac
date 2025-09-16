namespace V7.MessagingServices
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Provide message constant for consistency
    /// </summary>
    public static class MessageService
    {
        /// <summary>
        /// Successfully save data
        /// </summary>
        public const string SaveSuccess = "Successfully save data";
        public const string SaveFailed = "Failed to save data";
        public const string UpdateSuccess = "Successfully update data";
        public const string UpdateFailed = "Failed update data";
        public const string GetDataFailed = "Failed to get data";
        public const string GetDataSuccess = "Successfully get data";
        public const string DataNotFound = "Data not found";
        public const string DataAlreadyExist = "Data already exist";
        public const string DataNotValid = "Invalid Data";
        public const string LoginSuccess = "Login Successfull";
        public const string LoginFailed = "Login Failed";
        public const string ChangePasswordSuccess = "Successfully Change Password";
        public const string ChangePasswordFailed = "Failed to Change Password";
        public const string UserNotFound = "User Not Found";
        public const string UserPasswordNotCorrect = "User or password Not Correct";
        public const string UploadFailed = "Failed to upload";
        public const string UploadSuccess = "Successfully upload";
        public const string ProfileNotFound = "Profile not found";
        public const string EmailTokenFailed = "Email token failed";
        public const string DeleteFailed = "Failed to Delete";
        public const string DeleteSucceed = "Successfully Delete";

        //Upload File Log status
        public const string UploadLogProgress = "on progress";
        public const string UploadLogFailed = "failed";
        public const string UploadLogSuccess = "success";

        //Sending Email status
        public const string SendingEmailSuccess = "Email Sent";
        public const string SendingEmailFailed = "Email failed to Sent";

    }
}

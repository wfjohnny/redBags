using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model.AD
{
    [Serializable]
    public class AdUser : BaseModel
    {
        public AdUser()
        {

        }

        private string _usercode;
        private string _usergroupcode;
        private string _username;
        private string _userpwd;
        private int? _userstatus;
        private int? _isagentbound;
        private string _fullname;
        private string _appellation;
        private int? _sex;
        private string _employeeid;
        private string _usertel;
        private string _usermobile;
        private string _email;
        private string _position;
        private int? _usertype;
        private int? _isadmin;
        private int? _status;
        private string _remark;
        private string _creator;
        private DateTime? _createdate;
        private string _modifier;
        private DateTime? _modifydate;

        ///<sumary>
        /// 
        ///</sumary>
        public string UserCode
        {
            get { return _usercode; }
            set { _usercode = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string UserGroupCode
        {
            get { return _usergroupcode; }
            set { _usergroupcode = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string UserName
        {
            get { return _username; }
            set { _username = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string UserPwd
        {
            get { return _userpwd; }
            set { _userpwd = value; }
        }
        ///<sumary>
        /// 1 在线 2 离线 4 挂起
        ///</sumary>
        public int? UserStatus
        {
            get { return _userstatus; }
            set { _userstatus = value; }
        }
        ///<sumary>
        /// 是否绑定客服
        ///</sumary>
        public int? IsAgentBound
        {
            get { return _isagentbound; }
            set { _isagentbound = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string FullName
        {
            get { return _fullname; }
            set { _fullname = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string Appellation
        {
            get { return _appellation; }
            set { _appellation = value; }
        }
        ///<sumary>
        /// 1='男'  2 ='女'
        ///</sumary>
        public int? Sex
        {
            get { return _sex; }
            set { _sex = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string EmployeeID
        {
            get { return _employeeid; }
            set { _employeeid = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string UserTel
        {
            get { return _usertel; }
            set { _usertel = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string UserMobile
        {
            get { return _usermobile; }
            set { _usermobile = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        ///<sumary>
        /// 客服
        /// 物业前台
        /// 客服大使
        ///</sumary>
        public string Position
        {
            get { return _position; }
            set { _position = value; }
        }
        ///<sumary>
        /// admin,agent,TL...........
        ///</sumary>
        public int? UserType
        {
            get { return _usertype; }
            set { _usertype = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public int? IsAdmin
        {
            get { return _isadmin; }
            set { _isadmin = value; }
        }
        ///<sumary>
        /// 1 正常
        /// 2 禁用
        /// 4 删除
        ///</sumary>
        public int? Status
        {
            get { return _status; }
            set { _status = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string Creator
        {
            get { return _creator; }
            set { _creator = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public DateTime? CreateDate
        {
            get { return _createdate; }
            set { _createdate = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public string Modifier
        {
            get { return _modifier; }
            set { _modifier = value; }
        }
        ///<sumary>
        /// 
        ///</sumary>
        public DateTime? ModifyDate
        {
            get { return _modifydate; }
            set { _modifydate = value; }
        }
    }
}

using System.Collections.Generic;
using System.Net;
using System.Timers;

namespace AFTPLib.Managers {
    public class FirewallManager {
        private Dictionary<IPAddress, UserHistory> _connectionHistory;
        private Dictionary<IPAddress, BlockedUser> _blockedUsers;
        private readonly Timer _timer;
        private readonly int _newHistoryTimeout;
        private readonly int _newBlockedUserTimeout;
        private readonly int _maxConnections;
        private readonly int _maxFailedAttempts;
        
        public FirewallManager() {
            _blockedUsers = new Dictionary<IPAddress, BlockedUser>();
            _connectionHistory = new Dictionary<IPAddress, UserHistory>();
            _timer = new Timer();
            _timer.Interval = 1000;
            _timer.Elapsed += TimerOnElapsed;
        }

        public void Start() {
            _timer.Start();
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            foreach(BlockedUser blockedUser in _blockedUsers.Values) {
                blockedUser.Timeout--;
                if (blockedUser.Timeout <= 0) _blockedUsers.Remove(blockedUser.IPAddress);
            }
            
            foreach(UserHistory history in _connectionHistory.Values) {
                history.Timeout--;
                if (history.Timeout <= 0) _connectionHistory.Remove(history.IPAddress);
            }
        }

        public void Stop() {
            _timer.Stop();
        }

        public void AddConnection(IPAddress ipAddress, bool failed) {
            if (_connectionHistory.ContainsKey(ipAddress)) {
                _connectionHistory[ipAddress].Connections++;
                if (failed) _connectionHistory[ipAddress].FailedAttempts++;
                if (_connectionHistory[ipAddress].Connections >= _maxConnections ||
                    _connectionHistory[ipAddress].FailedAttempts >= _maxFailedAttempts) {
                    if (_blockedUsers.ContainsKey(ipAddress)) _blockedUsers[ipAddress].Timeout = _newBlockedUserTimeout;
                }
                _blockedUsers.Add(ipAddress, new BlockedUser(ipAddress, _newBlockedUserTimeout));
            } else {
                _connectionHistory.Add(ipAddress, new UserHistory(ipAddress, _newHistoryTimeout));
            }
        }

        public bool AllowConnection(IPAddress ipAddress, out int timeout) {
            timeout = 0;
            if (!_blockedUsers.ContainsKey(ipAddress)) return true;
            timeout = _blockedUsers[ipAddress].Timeout;
            return false;
        }
    }

    public class BlockedUser {
        public IPAddress IPAddress { get; }
        public int Timeout { get; set; }
        
        public BlockedUser(IPAddress ipAddress, int timeout) {
            IPAddress = ipAddress;
            Timeout = timeout;
        }
    }
    
    public class UserHistory {
        public IPAddress IPAddress { get; }
        public int Connections { get; set; }
        public int FailedAttempts { get; set; }
        public int Timeout { get; set; }

        public UserHistory(IPAddress ipAddress, int timeout) {
            IPAddress = ipAddress;
            Connections = 1;
            FailedAttempts = 0;
            Timeout = timeout;
        }
    }
}
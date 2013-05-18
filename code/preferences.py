import sqlite3,os

class SQLPreferences():
    __conn = None

    def __init__(self, dbname):
        self.__conn = sqlite3.connect(os.path.expanduser(dbname))
        if self.__conn == None:
            print 'Unable to open database'
            sys.exit(1)

        with self.__conn:
            cur = self.__conn.cursor()
            cur.execute("SELECT name FROM sqlite_master WHERE type='table' AND name='keystore'")
            data = cur.fetchone()
            if data == None:
                cur.execute("CREATE TABLE `keystore` (`name` TEXT NOT NULL, `data` BLOB NOT NULL, UNIQUE (name) ON CONFLICT REPLACE)")

    def readFromDB(self, key):
        cur = self.__conn.cursor()
        cur.execute("SELECT data FROM `keystore` WHERE name=?", (key,))
        data = cur.fetchone()
        if data != None:
            return data

    def writeToDB(self, key, value):
        with self.__conn:
            cur = self.__conn.cursor()
            if value == None:
                cur.execute('DELETE FROM `keystore` WHERE `name`=?', (key,))
            else:
                cur.execute('INSERT INTO `keystore` (`name`, `data`) VALUES (?, ?)', (key, value, ))

    def getPrefs(self):
        with self.__conn:
            cur = self.__conn.cursor()
            cur.execute("SELECT name,data FROM `keystore`")
            if cur != None:
                return cur.fetchall()
            else:
                return {}

    def __getattr__(self, name):
        if name[0] == '_':
            return self.__dict__[name]
        else:
            return self.readFromDB(name)
    
    def __setattr__(self, name, value):
        if name[0] == '_':
            self.__dict__[name] = value
        else:
            self.writeToDB(name, value)

    def __del__(self):
        if self.__conn:
            self.__conn.close()

#blarg = SQ3Preferences('~/.mycoolpath')

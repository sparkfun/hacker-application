-- --------------------------------------------------------

--
-- Table structure for table `aud2juror`
--

CREATE TABLE IF NOT EXISTS `aud2juror` (
  `aj_id` mediumint(6) unsigned NOT NULL auto_increment,
  `aud_id` mediumint(6) unsigned NOT NULL default '0',
  `juror_id` mediumint(6) unsigned NOT NULL default '0',
  `batch_id` mediumint(6) unsigned NOT NULL default '0',
  `assign_date` datetime default NULL,
  `response_date` datetime default NULL,
  `response_id` tinyint(1) unsigned NOT NULL default '0',
  `comments` text NOT NULL,
  `comments_admin` text NOT NULL,
  PRIMARY KEY  (`aj_id`),
  KEY `aud_id` (`aud_id`),
  KEY `juror_id` (`juror_id`),
  KEY `batch_id` (`batch_id`),
  KEY `response_id` (`response_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=8934 ;

-- --------------------------------------------------------

--
-- Table structure for table `aud2media`
--

CREATE TABLE IF NOT EXISTS `aud2media` (
  `aud_id` mediumint(6) NOT NULL default '0',
  `media_id` mediumint(6) NOT NULL default '0',
  PRIMARY KEY  (`aud_id`,`media_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `auditions`
--

CREATE TABLE IF NOT EXISTS `auditions` (
  `aud_id` mediumint(6) unsigned NOT NULL auto_increment,
  `person_id` mediumint(6) unsigned NOT NULL default '0',
  `order_id` mediumint(6) unsigned NOT NULL default '0',
  `online` tinyint(1) unsigned NOT NULL default '0',
  `al_id` smallint(3) unsigned NOT NULL default '0',
  `event_id` mediumint(6) unsigned NOT NULL default '0',
  `aud_pass` varchar(10) NOT NULL default 'Unpaid',
  `added` date default NULL,
  `rec_email_sent` tinyint(1) unsigned NOT NULL default '0',
  `result_email_sent` tinyint(1) unsigned NOT NULL default '0',
  `letter_prep` tinyint(1) unsigned NOT NULL default '0',
  `printed` tinyint(1) unsigned NOT NULL default '0',
  `finished` date default NULL,
  `locked` tinyint(1) unsigned NOT NULL default '0',
  `comments` text NOT NULL,
  `notes` text NOT NULL,
  PRIMARY KEY  (`aud_id`),
  KEY `person_id` (`person_id`),
  KEY `al_id` (`al_id`),
  KEY `rec_email_sent` (`rec_email_sent`),
  KEY `aud_pass` (`aud_pass`,`result_email_sent`),
  KEY `printed` (`printed`),
  KEY `event_id` (`event_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 AUTO_INCREMENT=9385 ;

-- --------------------------------------------------------

--
-- Table structure for table `aud_comments`
--

CREATE TABLE IF NOT EXISTS `aud_comments` (
  `cmt_id` mediumint(6) unsigned NOT NULL auto_increment,
  `cmt_text` varchar(255) NOT NULL,
  `cmt_priority` tinyint(2) unsigned NOT NULL default '0',
  PRIMARY KEY  (`cmt_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED AUTO_INCREMENT=12 ;

-- --------------------------------------------------------

--
-- Table structure for table `aud_juror`
--

CREATE TABLE IF NOT EXISTS `aud_juror` (
  `juror_id` mediumint(6) unsigned NOT NULL auto_increment,
  `person_id` mediumint(6) unsigned NOT NULL default '0',
  `name` varchar(35) NOT NULL,
  `instr` varchar(100) NOT NULL,
  `is_active` tinyint(1) NOT NULL default '1',
  `max_month` tinyint(3) unsigned NOT NULL default '50',
  `vac_start` date NOT NULL default '0000-00-00',
  `vac_end` date NOT NULL default '0000-00-00',
  PRIMARY KEY  (`juror_id`),
  KEY `instr` (`instr`,`is_active`),
  KEY `person_id` (`person_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED AUTO_INCREMENT=8934 ;

-- --------------------------------------------------------

--
-- Table structure for table `aud_juror2level`
--

CREATE TABLE IF NOT EXISTS `aud_juror2level` (
  `juror_id` mediumint(6) unsigned NOT NULL default '0',
  `al_id` mediumint(6) unsigned NOT NULL default '0',
  PRIMARY KEY  (`juror_id`,`al_id`)
) ENGINE=MyISAM DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `aud_level`
--

CREATE TABLE IF NOT EXISTS `aud_level` (
  `al_id` smallint(3) unsigned NOT NULL auto_increment,
  `instr` varchar(40) NOT NULL,
  `level` varchar(20) NOT NULL,
  `repertoire` varchar(150) NOT NULL,
  `approval` varchar(20) NOT NULL,
  `al_order` tinyint(1) unsigned NOT NULL default '1',
  PRIMARY KEY  (`al_id`),
  KEY `instr` (`instr`,`level`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED AUTO_INCREMENT=28 ;

-- --------------------------------------------------------

--
-- Table structure for table `aud_response`
--

CREATE TABLE IF NOT EXISTS `aud_response` (
  `response_id` tinyint(1) unsigned NOT NULL auto_increment,
  `response` varchar(30) NOT NULL,
  `points` tinyint(1) NOT NULL default '0',
  `response_order` tinyint(1) unsigned NOT NULL default '0',
  PRIMARY KEY  (`response_id`)
) ENGINE=MyISAM  DEFAULT CHARSET=utf8 ROW_FORMAT=FIXED AUTO_INCREMENT=8 ;

--
-- Dumping data for table `aud_response`
--

INSERT INTO `aud_response` (`response_id`, `response`, `points`, `response_order`) VALUES
(0, '', 0, 0),
(1, 'Yes', 1, 1),
(2, 'No', -1, 3),
(3, 'Can''t play video', 0, 7),
(4, 'Undecided', 0, 4),
(5, 'Wrong repertoire', 0, 6),
(6, 'Yes, with reservations', 1, 2),
(7, 'Recording quality too poor', 0, 5);

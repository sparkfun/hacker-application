<?php

/*

see audition.sql for db schema

*/

require_once($c->get('library_path').'Media.php');
require_once($c->get('library_path').'MediaSlideshow.php');

// ------------------------------------------------------ class AuditionJurorVote

// helper functions

function e($txt) {
	// do htmlspecialchars then echo, but don't double-encode!
	echo str_replace('&amp;#', '&#', htmlspecialchars($txt));
}

function getAuditionJurorVoteById($id) {

    $db = SqlConnection::instance();

	// db->prepare escapes parameters with mysqli_real_escape_string
    $query = $db->prepare("SELECT *
		FROM aud2juror
		WHERE aj_id=%d",
		$id);

    if ($row = $db->getRecord($query)) {
        return new AuditionJurorVote($row);
    }

    return new AuditionJurorVote(array());
}

// find all votes still needing a decision for a given juror
// returns array of AuditionJurorVote objects
function getPendingAuditionJurorVotes($juror_id) {

	$db = SqlConnection::instance();

	$query = $db->prepare("SELECT *
		FROM aud2juror
		WHERE juror_id=%d
		AND response_id=0
		ORDER BY assign_date ASC",
		$juror_id);

	$db->execute($query);
	
	$votes = array();
	
	while ($row = $db->readRecord()) {
		$votes[$row['aj_id']] = new AuditionJurorVote($row);
	}
	
	return $votes;
}

class AuditionJurorVote {

	private $params	= array();
	private $db = null;
    
    public function __construct($params) {
		
		$this->db = SqlConnection::instance();
	
		$this->params['aj_id']   		= 0;
		$this->params['aud_id']  		= 0;
		$this->params['juror_id']  		= 0;
		$this->params['batch_id']  		= 2; // online audition batch
		$this->params['assign_date']  	= null;
		$this->params['response_date'] 	= null;
		$this->params['response_id'] 	= 0;
		$this->params['comments'] 		= '';
		$this->params['comments_admin'] = '';
		
		$this->__set($params);
    }
	
	/*
	set data
	@param mixed	$params key/value array to set many params or string key when using $val
	@param mixed	$val	setting a single param
	*/	
	public function __set($params, $val = null) {
	
		if (!is_null($val) && !is_array($params)) {
            $params = array($params => $val);
        }
		
		if (isset($params['id'])) {
			$this->params['aj_id']				= (int)$params['id'];
		}
		if (isset($params['aj_id'])) {
			$this->params['aj_id']				= (int)$params['aj_id'];
		}
		if (isset($params['aud_id'])) {
			$this->params['aud_id']				= (int)$params['aud_id'];
		}
		if (isset($params['juror_id'])) {
			$this->params['juror_id']			= (int)$params['juror_id'];
		}
		if (isset($params['batch_id'])) {
			$this->params['batch_id']			= (int)$params['batch_id'];
		}
		if (isset($params['assign_date'])) {
			$this->params['assign_date']		= formatTime('Y-m-d H:i:s', $params['assign_date']);
		}
		if (isset($params['response_date'])) {
			$this->params['response_date']		= formatTime('Y-m-d H:i:s', $params['response_date']);
		}
		if (isset($params['response_id'])) {
			$this->params['response_id']		= (int)$params['response_id'];
		}
		// comments may come in an array, convert to string
		if (!empty($params['comments']) && is_array($params['comments'])) {
			$params['comments'] = implode($params['comments'], ', ');
		}
		if (isset($params['comments'])) {
			$this->params['comments']			= $params['comments'];
		}
		if (isset($params['comments_admin'])) {
			$this->params['comments_admin']		= $params['comments_admin'];
		}
	}
	
	public function __get($key) {
		if ($key == 'id') {
			$key = 'aj_id';
		}
        if (isset($this->params[$key])) {
            return $this->params[$key];
        }
		return false;
    }
	
	// save juror vote
	// then update audition final status
	public function submitVote() {
		
		$query = $this->db->prepare("UPDATE aud2juror
			SET response_id=%d, comments='%s', comments_admin='%s', response_date=NOW()
			WHERE aj_id=%d",
			$this->response_id, $this->comments, $this->comments_admin,
			$this->id);
			
		if ($this->db->execute($query, true, 'submit vote')) {
		
			$this->combineJurorComments();
			
			$this->checkForProblems();
			
			$this->calcAudStatus();
			
			return true;
		}
		
		return false;
	}
	
	/*

	determine final status for all pending auditions
	then assign any still pending auditions to a new juror

	each yes vote = 1 point
	each no vote = -1 point
	each neutral vote = 0 points

	final yes requires more yes votes than no votes
	or a yes on the first vote
	sum_points > 0

	final no requires more no votes than yes votes, with at least 2 votes
	2 no votes in a row without a yes
	num_vote == 2 && sum_points == -2
	or 2 out of 3 votes for no (no, yes, no in order)
	num_vote > 2 && sum_points < 0

	*/
	
	public function calcAudStatus() {
		
		// find all pending auditions
		// with at least one juror vote

		$query = "SELECT a.aud_id, COUNT(aj.aj_id) AS num_vote, SUM(ar.points) AS sum_points
			FROM auditions a
			JOIN aud2juror aj ON a.aud_id=aj.aud_id AND aj.response_date IS NOT NULL
			JOIN aud_response ar ON aj.response_id=ar.response_id
			WHERE a.aud_pass='Pending'
			GROUP BY a.aud_id
			ORDER BY sum_points";
		
		$this->db->execute($query);
		
		while ($row = $this->db->readRecord()) {
		
			// pass = yes, more yes than no
			if ($row['sum_points'] > 0) {
			
				$query = $this->db->prepare("UPDATE auditions a
					SET a.aud_pass='Yes', a.finished=NOW(), a.locked=1
					WHERE a.aud_id=%d",
					$row['aud_id']);
				
				$this->db->execute($query, true, 'accept audition', $row['aud_id'], 1);
			
			// pass = no, more no with more than 1 vote
			} elseif (($row['num_vote'] == 2 && $row['sum_points'] == -2)
				|| ($row['num_vote'] > 2 && $row['sum_points'] < 0)) {
			
				$query = $this->db->prepare("UPDATE auditions a
					SET a.aud_pass='No', a.finished=NOW(), a.locked=1
					WHERE a.aud_id=%d",
					$row['aud_id']);
				
				$this->db->execute($query, true, 'reject audition', $row['aud_id'], 1);
			
			// needs more votes
			} else {
			
				// do nothing
			}
		}
		
		// assign any auditions still pending to a new juror
		$this->assignAudJuror();
		
		return true;		
	}
	
	/*

	assign all pending auditions to a juror for review
	
	this static method seems clunky when used outside the class
	maybe it belongs somewhere else?
	
	*/
	
	public static function assignAudJuror() {
	
		$db = SqlConnection::instance();
		
		// find all pending auditions
		// with videos that have already been converted for streaming (mc.do_convert=0)
		// but not currently assigned to a juror for review (aj.aj_id IS NULL)
		
		$query = "SELECT a.aud_id, a.al_id
			FROM auditions a
			JOIN aud_level al ON a.al_id=al.al_id
			JOIN people p ON a.person_id=p.person_id
			JOIN aud2media am ON a.aud_id=am.aud_id
			JOIN media_convert mc ON am.media_id=mc.media_id
			LEFT JOIN aud2juror aj ON a.aud_id=aj.aud_id AND aj.response_date IS NULL
			WHERE a.aud_pass='Pending'
			AND mc.do_convert=0
			AND aj.aj_id IS NULL
			GROUP BY a.aud_id DESC";
			
		$db->execute($query);
		
		while ($row = $db->readRecord()) {
		
			$params = array();
			$params['aud_id'] = $row['aud_id'];
			$params['assign_date'] = now();
		
			// select juror by random
			// only those able to review this type of audition (ajl.al_id=$row['aud_id'])
			// exclude those on vacation hold (CURDATE() NOT BETWEEN j.vac_start AND j.vac_end)
			// exclude those who have already review this audition (aj2.aud_id IS NULL)
			// only those who have not reached monthly max (HAVING max_month > num_month)
			$query = $db->prepare("SELECT j.juror_id, j.max_month, COUNT(aj.aud_id) AS num_month
				FROM aud_juror j
				JOIN aud_juror2level ajl ON j.juror_id=ajl.juror_id
				LEFT JOIN aud2juror aj ON j.juror_id=aj.juror_id AND aj.assign_date >= (CURDATE() - INTERVAL 1 MONTH)
				LEFT JOIN aud2juror aj2 ON j.juror_id=aj2.juror_id AND aj2.aud_id=%d
				WHERE ajl.al_id=%d
				AND j.is_active=1
				AND CURDATE() NOT BETWEEN j.vac_start AND j.vac_end
				AND aj2.aud_id IS NULL
				GROUP BY j.juror_id
				HAVING max_month > num_month
				ORDER BY RAND() LIMIT 1",
				$row['aud_id'], $row['al_id']);
			
			$params['juror_id'] = $db->getFirstCell($query);
			
			// assign audition to juror
			$vote = new AuditionJurorVote($params);
			$vote->updateDb();
		}
		
		return true;
	}
	
	// save new vote assignment to db
	// or update vote assignment (not currently needed)
	public function updateDb() {
        
        if (!$this->id) { // no ID = new, must insert
            $query = "INSERT INTO aud2juror SET ";
        } else {
            $query = "UPDATE aud2juror SET ";
        }
    
        $query .= $this->db->prepare("aud_id=%d, juror_id=%d, batch_id=%d, assign_date='%s'",
            $this->aud_id, $this->juror_id, $this->batch_id, $this->assign_date);
        
        if ($this->id) {
            $query .= $this->db->prepare(" WHERE aj_id=%d", $this->id);
        }
        
        if ($this->db->execute($query)) {
		
            if (!$this->id) {
                $this->id = $this->db->getLastSequence();
            }
			
			return true;
        }
        
        return false;
    }

	// combine standard comments from all jurors
	// into 5 most popular for letter
	private function combineJurorComments() {

		$query = $this->db->prepare("SELECT ac.cmt_text, COUNT(*) AS num
			FROM aud_comments ac
			JOIN aud2juror aj ON aj.comments LIKE CONCAT('%%', ac.cmt_text, '%%')
			JOIN auditions a ON aj.aud_id=a.aud_id
			WHERE a.aud_id=%d
			AND a.aud_pass IN('Pending', 'Problems')
			GROUP BY ac.cmt_id
			ORDER BY num DESC, ac.cmt_priority ASC LIMIT 5",
			$this->aud_id);
			
		$this->db->execute($query);
		
		if ($this->db->numRows()) {
		
			$comment_merge = array();
			
			while ($row = $this->db->readRecord()) {
				$comment_merge[] = $row['cmt_text'];
			}
			
			$comment_merge_str = implode(', ', $comment_merge);
			
			$query = $this->db->prepare("UPDATE auditions
				SET comments='%s'
				WHERE aud_id=%d",
				$comment_merge_str, $this->aud_id);
				
			$this->db->execute($query, false);
		}
		
		return true;
	}
	
	// move to problems group if juror vote indicates problem
	// must be moved back to pending by admin when problems are fixed
	// 3: can't play video
	// 5: wrong repertoire
	// 7: recording quality too poor
	private function checkForProblems() {
		
		if (in_array($this->response_id, array(3, 5, 7))) {
		
			// set audition status as problems
			$query = $this->db->prepare("UPDATE auditions SET aud_pass='Problems' WHERE aud_id=%d", $this->aud_id);
			$this->db->execute($query, 'move to problems');
			
			// put reported problem in audition notes field
			$query = $this->db->prepare("UPDATE auditions a
				JOIN aud2juror aj ON a.aud_id=aj.aud_id
				JOIN aud_response ar ON aj.response_id=ar.response_id
				SET a.notes=TRIM(CONCAT_WS(' ', a.notes, ar.response))
				WHERE a.aud_pass='Problems'
				AND aj.aj_id=%d",
				$this->id);

			$this->db->execute($query, false);
			
			// put audition in problems group
			$query = $this->db->prepare("INSERT INTO aud2juror (batch_id, aud_id)
				VALUES (1, %d)",
				$this->aud_id);

			$this->db->execute($query, false);
		}
		
		return true;
	}

} // end class AuditionJurorVote

/* ====================================================================================

Classes related to AuditionJurorVote are below, but you can probably stop reading here

*/

// ------------------------------------------------------ class Audition

function getAuditionById($id) {

    $db = SqlConnection::instance();
	
    $query = $db->prepare("SELECT a.*, al.*, p.firstname, p.lastname
        FROM auditions a
		JOIN aud_level al ON a.al_id=al.al_id
		JOIN people p ON a.person_id=p.person_id
        WHERE a.aud_id=%d LIMIT 1",
        $id);

    if ($row = $db->getRecord($query)) {
        return new Audition($row);
    }

    return new Audition(array());
}

class Audition {

	private $params	= array();
	private $db = null;
    
    public function __construct($params) {
	
		$this->db = SqlConnection::instance();
	
        $this->params['aud_id']		= 0;
		$this->params['person_id']	= 0;
		$this->params['order_id']   = 0;
		
		$this->params['online']		= 0;
        
		$this->params['al_id']		= 0;
        $this->params['aud_pass']   = 'Unpaid';
        
        $this->params['added']      = now();
		
		$this->params['rec_email_sent']	= 0;
		$this->params['result_email_sent']	= 0;
		$this->params['letter_prep']	= 0;
		$this->params['printed']		= 0;
		
		$this->params['finished']	= null;
		$this->params['locked']		= 0;
		
		$this->params['comments']	= '';
		$this->params['notes']		= '';
		
		// from aud_level table
		$this->params['instr']		= '';
		$this->params['level']		= '';
		$this->params['repertoire']		= '';
		$this->params['approval']		= '';
		
		// from people table
		$this->params['firstname']	= '';
		$this->params['lastname']	= '';
		
		$this->__set($params);
    }
	
	public function __set($params, $val = null) {
	
		if (!is_null($val) && !is_array($params)) {
            $params = array($params => $val);
        }
		
		if (isset($params['id'])) {
			$this->params['aud_id']			= (int)$params['id'];
		}
		if (isset($params['aud_id'])) {
			$this->params['aud_id']			= (int)$params['aud_id'];
		}
		if (isset($params['person_id'])) {
			$this->params['person_id']		= (int)$params['person_id'];
		}
		if (isset($params['order_id'])) {
			$this->params['order_id']		= (int)$params['order_id'];
		}
		
		if (isset($params['online'])) {
			$this->params['online']			= (int)$params['online'];
		}
		
		if (isset($params['al_id'])) {
			$this->params['al_id']			= (int)$params['al_id'];
		}
		if (isset($params['aud_pass'])) {
			$this->params['aud_pass']		= $params['aud_pass'];
		}
		
		if (isset($params['added'])) {
			$this->params['added'] 			= formatTime('Y-m-d H:i:s', $params['added']);
		}

		if (isset($params['rec_email_sent'])) {
			$this->params['rec_email_sent']	= (int)$params['rec_email_sent'];
		}
		if (isset($params['result_email_sent'])) {
			$this->params['result_email_sent']	= (int)$params['result_email_sent'];
		}
		if (isset($params['letter_prep'])) {
			$this->params['letter_prep']	= (int)$params['letter_prep'];
		}
		if (isset($params['printed'])) {
			$this->params['printed']		= (int)$params['printed'];
		}
		
		if (in_array($this->aud_pass, array('Yes', 'No'))) {
		
			$params['locked'] = 1;
			
			if (empty($params['finished'])) {
				$params['finished'] = date('Y-m-d');
			}
		}
		
		if (isset($params['finished'])) {
			$this->params['finished']		= formatTime('Y-m-d', $params['finished']);
		}
		if (isset($params['locked'])) {
			$this->params['locked']			= (int)$params['locked'];
		}
		
		if (isset($params['comments'])) {
			$this->params['comments']		= $params['comments'];
		}
		if (isset($params['notes'])) {
			$this->params['notes']			= $params['notes'];
		}
		
		if (isset($params['instr'])) {
			$this->params['instr']			= $params['instr'];
		}
		if (isset($params['level'])) {
			$this->params['level']			= $params['level'];
		}
		if (isset($params['repertoire'])) {
			$this->params['repertoire']		= $params['repertoire'];
		}
		if (isset($params['approval'])) {
			$this->params['approval']		= $params['approval'];
		}
		
		if (isset($params['firstname'])) {
			$this->params['firstname']		= $params['firstname'];
		}
		if (isset($params['lastname'])) {
			$this->params['lastname']		= $params['lastname'];
		}
	}
	
	public function __get($key) {
		if ($key == 'id') {
			$key = 'aud_id';
		}
        if (isset($this->params[$key])) {
            return $this->params[$key];
        }
		return false;
    }
	
	public function adminUrl() {
		return '/admin/auditions/'.$this->id.'/';
	}
	
	public function updateDb() {
        
        if (!$this->id) {
            $query = "INSERT INTO auditions SET ";
            $action = 'add';
        } else {
            $query = "UPDATE auditions SET ";
            $action = 'update';
        }
    
        $query .= $this->db->prepare("person_id=%d, order_id=%d,
			online=%d, al_id=%d, aud_pass='%s',
			printed=%d, notes='%s'",
            $this->person_id, $this->order_id,
			$this->online, $this->al_id, $this->aud_pass,
			$this->printed, $this->notes);
        
        if (!$this->id) {
            $query .= ", added=NOW()";
        } else {
            $query .= $this->db->prepare(" WHERE aud_id=%d", $this->id);
        }
        
        if ($this->db->execute($query)) {
		
            if (!$this->id) {
                $this->id = $this->db->getLastSequence();
            }
			
			// remove from problems group
			// any auditions that are no longer problems			
			$query = "DELETE aj.* FROM aud2juror aj
				JOIN auditions a ON aj.aud_id=a.aud_id
				WHERE a.aud_pass!='Problems'
				AND aj.batch_id=1
				AND aj.assign_date IS NULL";
			
			$this->db->execute($query, false);

            return true;
        }
        
        return false;
    }
	
	public function reopen() {
		
		$query = $this->db->prepare("UPDATE auditions
			SET aud_pass='Pending'
			WHERE aud_id=%d",
			$this->id);

		$this->db->execute($query, true, 're-open for more jurors');
		
		AuditionJurorVote::assignAudJuror();
	}
	
	public function delete() {
	
		// only delete unpaid auditions
		$query = $this->db->prepare("DELETE FROM auditions WHERE aud_id=%d AND aud_pass='Unpaid'", $this->id);

		if ($this->db->execute($query)) {
			return true;
		}
		
		return false;
	}
	
	public function embedVideo() {
	
		$query = $this->db->prepare("SELECT media_id FROM aud2media WHERE aud_id=%d", $this->id);

		$this->db->execute($query);

		if ($this->db->numRows() > 1) {
			
			$params['audition'] = $this->id;
			$params['type'] = 'video';
			$params['size'] = '640x640';
			$slideshow = new MediaSlideshow($params);
			
			echo $slideshow->embed();

		} else {

			$m = $this->db->readRecord();

			$media = getMediaById($m['media_id']);

			if ($media->getId()) {
				echo $media->embed(array('size' => '640x480'));
			}

		}

		$this->db->freeResult();
	}

} // end class Audition

// ------------------------------------------------------ class AuditionJuror

function getAuditionJurorById($id) {

    $db = SqlConnection::instance();
	
	$where = $db->prepare("j.juror_id=%d", $id);
	// use person_id for $id format p123
	if (strpos($id, 'p') !== false) {
		$where = $db->prepare("j.person_id=%d", str_replace('p', '', $id));
	}
	
    $query = "SELECT j.juror_id, p.firstname, p.lastname
		FROM aud_juror j
		JOIN people p ON j.person_id=p.person_id
		WHERE $where";

    if ($row = $db->getRecord($query)) {
        return new AuditionJuror($row);
    }

    return new AuditionJuror(array());
}

class AuditionJuror {

	private $params	= array();
	private $db = null;
    
    public function __construct($params) {
	
		$this->db = SqlConnection::instance();
	
		$this->params['juror_id']   = 0;
		$this->params['person_id']  = 0;
		$this->params['instr']		= '';
		$this->params['is_active']  = 0;
		$this->params['max_month']  = 50;
		$this->params['vac_start']  = '0000-00-00';
		$this->params['vac_end'] 	= '0000-00-00';
		
		// from people table
		$this->params['firstname']		= '';
		$this->params['lastname']		= '';
		
		$this->__set($params);
    }
	
	public function __set($params, $val = null) {
	
		if (!is_null($val) && !is_array($params)) {
            $params = array($params => $val);
        }
		
		if (isset($params['id'])) {
			$this->params['juror_id']	= (int)$params['id'];
		}
		if (isset($params['juror_id'])) {
			$this->params['juror_id']	= (int)$params['juror_id'];
		}
		if (isset($params['person_id'])) {
			$this->params['person_id']	= (int)$params['person_id'];
		}
		if (isset($params['instr'])) {
			$this->params['instr']		= $params['instr'];
		}
		if (isset($params['is_active'])) {
			$this->params['is_active']	= (int)$params['is_active'];
		}
		if (isset($params['max_month'])) {
			$this->params['max_month']	= (int)$params['max_month'];
		}
		
		// clear vacation setting if it's already passed
		if (!empty($params['vac_end']) && formatTime('Y-m-d', $params['vac_start']) < date('Y-m-d')) {
			$params['vac_start'] = '0000-00-00';
			$params['vac_end'] = '0000-00-00';
		}
	
		if (isset($params['vac_start'])) {
			$this->params['vac_start']	= formatTime('Y-m-d', $params['vac_start']);
		}
		if (isset($params['vac_end'])) {
			$this->params['vac_end']	= formatTime('Y-m-d', $params['vac_end']);
		}
		
		if (isset($params['firstname'])) {
			$this->params['firstname']	= $params['firstname'];
		}
		if (isset($params['lastname'])) {
			$this->params['lastname']	= $params['lastname'];
		}
	}
	
	public function __get($key) {
		if ($key == 'id') {
			$key = 'juror_id';
		}
        if (isset($this->params[$key])) {
            return $this->params[$key];
        }
		return false;
    }
	
	public function adminUrl() {
		return '/admin/auditions/jurors/'.$this->id.'/';
	}
	
	public function updatePrefs() {
		
		$query = $this->db->prepare("UPDATE aud_juror SET max_month=%d, vac_start='%s', vac_end='%s' WHERE juror_id=%d",
			$this->gmax_month, $this->vac_start, $this->vac_end, $this->id);
			
		if ($this->db->execute($query, true, 'update prefs')) {
			return true;
		}
		
		return false;
	}
	
	public function updateDb() {
        
        if (!$this->id) { // no ID = new, must insert
            $query = "INSERT INTO aud_juror SET ";
        } else {
            $query = "UPDATE aud_juror SET ";
        }
    
        $query .= $this->db->prepare("person_id=%d, instr='%s', is_active=%d",
            $this->person_id, $this->instr, $this->is_active);
        
        if ($this->id) {
            $query .= $this->db->prepare(" WHERE juror_id=%d", $this->id);
        }
        
        if ($this->db->execute($query)) {
		
            if (!$this->id) {
                $this->id = $this->db->getLastSequence();
            }
			
			return $this->updatePrefs();
        }
        
        return false;
    }

} // end class AuditionJuror

?>
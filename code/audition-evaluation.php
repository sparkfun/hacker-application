<?php

/*

This is where audition jurors view videos and submit their decisions.

After the juror has gone through their entire queue, they can
update their vacation settings.

*/

// haven't refactored everything to use magic __get yet!
require_once($_SERVER['DOCUMENT_ROOT'].'/config.php');
require_once($c->get('library_path').'Auth.php');
$auth = new Auth(156); // audition juror group

// audition-related classes
require_once($c->get('library_path').'Audition.class.php');

$submit_url = '/admin/auditions/queue/';

// find juror
$juror = getAuditionJurorById('p'.$auth->get('person_id'));
// it's possible the juror group could be out of sync with current juror list
if (!$juror->id) {
	echo "Hey! You're not really a juror!";
	exit;
}

// get standard comments
$query = "SELECT * FROM aud_comments ORDER BY cmt_id";
$standard_cmts = $db->getRecords($query, 'cmt_id');

// get decision options
$query = "SELECT * FROM aud_response ORDER BY response_order";
$responses = $db->getRecords($query, 'response_id');

// which audition to show?
$show_aj_id = 0; // first in list
if (!empty($_GET['id'])) { // user choice
	$show_aj_id = (int)$_GET['id'];
}

// ------------------------------------------------------- actions (process user input)

if (!empty($_POST['action'])) {

	// sanitize user input
	$clean = clean_input($_POST);

	// --------------------------------------------------- submit_vote
	
	if ($clean['action'] == 'submit_vote') {

		// go back to last audition if there are any problems
		$show_aj_id = $clean['aj_id'];
		
		$vote = getAuditionJurorVoteById($show_aj_id);
		// don't let juror vote on auditions they haven't been assigned
		if ($vote->juror_id != $juror->id) {
			echo 'You are not authorized to vote on this audition.';
			exit;
		}
		
		$vote->__set($clean);

		if (!$vote->response_id || !ctype_digit($vote->response_id)) {
			$log->addErr('You must select a decision.');
		}
		
		if ($vote->response_id == 2 && !$vote->comments) { // 2 = no
			$log->addErr("You must select some areas for improvement when you don't pass an audition.");
		}
		
		// no problems, continue to save vote
		if ($log->noProb()) {
		
			// save decision
			// also calculates final status (or not) for audition
			$vote->submitVote();
			
			$log->addSuccess("Audition #{$vote->id} decision \"{$responses[$vote->response_id]['response']}\" saved.");
			
			$show_aj_id = 0; // go to first pending audition in list
			$clean = array(); // clear previous answer	
		}

	// --------------------------------------------------- update_prefs
	
	} elseif ($clean['action'] == 'update_prefs') {
	
		$juror->__set($clean);
		
		$juror->updatePrefs();
		
		$log->addSuccess('Preferences saved.');
	}

} // end actions

// ------------------------------------------------------- views (what to show next?)

// find all assigned auditions for juror

$votes_queue = getPendingAuditionJurorVotes($juror->id);

$queue_total = count($votes_queue);

// show an audition to evaluate
if ($queue_total) {

	if ($show_aj_id) { // show specific audition
		$vote = $votes_queue[$show_aj_id];
		unset($votes_queue[$show_aj_id]);
	} else { // just show whichever is next
		$vote = array_shift($votes_queue);
	}
	$queue_total--;
	
	$audition = getAuditionById($vote->aud_id);
	
	$show = 'video';

// no more auditions to evaluate, show vacation preferences form	
} else {

	$show = 'prefs';

}

// ------------------------------------------------------- html

// header

$options['extra_css'] = 'admin-audqueue.css';
admin_header('Audition Queue for '.$juror->lastname, 'no_nav', $options);

// report any errors/confirmations
$log->reportAll();

// ------------------------------------------------------- video
// show juror an audition video to review

if ($show == 'video') {

?>

<div class="col25 eval_video">
<?php $audition->embedVideo(); ?>
</div><!-- end .col25 .eval_video -->

<div class="col15 eval_form">

<h2><?php e($audition->instr); ?> <?php e($audition->level); ?> Audition #<?php e($audition->id); ?></h2>

<p><strong>Repertoire:</strong> <?php e($audition->repertoire); ?></p>

<form action="<?php e($submit_url); ?>" method="post">
<input type="hidden" name="action" value="submit_vote" />
<input type="hidden" name="aj_id" value="<?php e($vote->id); ?>" />
<input type="hidden" name="aud_id" value="<?php e($audition->id); ?>" />

<h4>Your Decision:</h4>
<p><select id="response_id" name="response_id">
<?php

foreach ($responses as $r) {
    echo '<option value="'.$r['response_id'].'"';
	if (isset($clean['response_id']) && $clean['response_id'] == $r['response_id']) { echo 'selected="selected"'; }
	echo '>';
	e($r['response']);
	echo '</option>';
}

?>
</select></p>

<div id="juror_comments">

<h4>Areas for Improvement:</h4>
<p>
<?php foreach ($standard_cmts as $cmt) { ?>
<label class="checkbox"><input type="checkbox" name="comments[]" value="<?php e($cmt['cmt_text']); ?>"
<?php if (isset($clean['comments']) && in_array($cmt['cmt_text'], $clean['comments'])) { echo 'checked="checked"'; } ?> />
<?php e($cmt['cmt_text']); ?></label><br />
<?php } ?>
</p>

</div>

<h4>Other Comments:</h4>
<p>If not giving a "Yes", <strong>kind, constructive comments</strong> to help the applicant improve their technique are strongly encouraged. Your unedited comments may be forwarded anonymously to the applicant.</p>
<p><textarea name="comments_admin" cols="500" rows="6"><?php if (!empty($clean['comments_admin'])) { e($clean['comments_admin']); } ?></textarea></p>

<p><input type="submit" value="Submit Decision" class="update med_action" /></p>

</form>

<?php

if ($queue_total) {	
		
?>

<hr />

<p><strong><?php e($queue_total); ?> more auditions after this.</strong><br />
Submit your decision to go to the next audition, or skip to one of these:</p>

<?php foreach ($votes_queue as $vote) {

$aud = getAuditionById($vote->aud_id);

?>
<p><a href="<?php e($submit_url); ?><?php e($vote->id); ?>"><?php e($aud->instr); ?> <?php e($aud->level); ?> Audition #<?php e($vote->id); ?></a></p>
<?php } ?>

<?php } ?>

<p>&nbsp;</p>
<p><em>Having technical problems? 
Email <?php echo link_email($c->get('admin_email')); ?> for help.</em></p>

</div><!-- end .col15 .eval_form -->

<script>

// show standard comment choices only for No and unsure decisions

var juror_status = $('response_id');
var juror_comments = $('juror_comments');

function showJurorComments() {
	if (juror_status.value == 2 || juror_status.value == 6 || juror_status.value == 5) {
		juror_comments.style.display = 'block';
	} else {
		juror_comments.style.display = 'none';
	}
}

addEvent(juror_status, 'change', showJurorComments);

showJurorComments();

</script>

<?php

// ------------------------------------------------------- prefs
// no auditions to evaluate, ask juror about preferences

} else {

?>

<div class="success">
<p>Thank you! You have no more auditions to evaluate. We'll email you when there are more.</p>
</div>

<h2>Your Future Availability</h2>

<form action="<?php e($submit_url); ?>" method="post">
<input type="hidden" name="action" value="update_prefs" />

<h4>Maximum number of auditions you want to evaluate per month&#8212;0 to 100.</h4>
<p>We won't send you more than you can handle!</p>

<p><label for="max_month">Max per month:</label>
<input type="text" id="max_month" name="max_month" value="<?php e($juror->max_month); ?>" size="3" maxlength="3"/></p>

<h4>Will you be away from your computer soon?</h4>
<p>You can put a temporary "vacation hold" on audition evaluations, and we won't send you any more until you return. Configure your vacation start and end dates below.</p>

<p><label for="vac_start">Start date:</label>
<input type="text" id="vac_start" name="vac_start" value="<?php e(formatTime('M j Y', $juror->vac_start)); ?>" /></p>

<p><label for="vac_end">End date:</label>
<input type="text" id="vac_end" name="vac_end" value="<?php e(formatTime('M j Y', $juror->vac_end)); ?>" /></p>

<p class="hanglabel"><input type="submit" value="Save" class="update med_action" /></p>

</form>

<?php

}

admin_footer('admin_nav');

?>
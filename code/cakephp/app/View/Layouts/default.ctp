<?php
/*
* Event Finder - PHP
* @author Austin Stoltzfus <astoltzf@gmail.com>
*/
?>
<!DOCTYPE html>
<html>
<head>
	<?php echo $this->Html->charset(); ?>
	<title>Event Finder App</title>
	<?php
		echo $this->Html->meta('icon');

        echo $this->Html->css('event_finder.less?', 'stylesheet/less');
        echo $this->Html->script('less');
        echo $this->Html->script('//ajax.googleapis.com/ajax/libs/jquery/1.2.6/jquery.min.js');

		echo $this->fetch('meta');
		echo $this->fetch('css');
		echo $this->fetch('script');
	?>
</head>
<body>
	<div id="container">
        <div id="navbar">
            <div id="navbar-inner">
                    <ul class="nav">
                        <li><?php $this->Html->link('Event Finder', 'Events/search_events'); ?></li>
                        <li><a href="#about">About</a></li>
                    </ul>
            </div>
        </div>
		<div id="content">

			<?php echo $this->Session->flash(); ?>

			<?php echo $this->fetch('content'); ?>
		</div>
		<div id="footer">
		</div>
	</div>
	<?php echo $this->element('sql_dump'); ?>
</body>
</html>

<?php $this->extend('Events/event_map.ctp'); ?>

<h1>Event List</h2>

<div class="content-wrapper background-blue">
    <div class="content-container">
        <div class="event-center">
            <?php echo $this->Session->flash(); ?>
        </div>
    </div>
</div>

<?php foreach ($events as $event); ?>
    <?php $event_detail = $event['Event']; ?>

    <div class="event-list-item">
        <div class="event-center">
            <h2 class="name"><?php echo $event_detail['name']; ?></h2>
        </div>
        <div class="event-left">
            <p><?php echo $event_detail['group_name']; ?></p>
            <p><?php echo $event_detail['time']; ?></p>
            <p><?php echo $event_detail['full_address']; ?></p>
        </div>
        <div class="event-right">
            <p><?php echo $event_detail['short_description']; ?></p>
        </div>
        <div class="event-footer">
            <?php $this->fetch('eventMapButton'); ?>
            <?php $this->Html->link('View Details', 'events/event_details', array('id' => $event_detail['id']), 'class' => 'btn'; ?>
        </div>
    </div>
<?php endforeach; ?>

<?php $this->fetch('eventMap'); ?>

<?php unset($event); ?>



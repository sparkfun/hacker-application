<?php $this->extend('Events/event_map.ctp'); ?>

<?php $event_detail = h($event['Event']); ?>
<div class="content-wrapper background-blue">
    <div class="content-container event-details">
        <div class="event-center">
            <h1><?php echo $event_detail['name']; ?></h1>
            <h3>Status: <?php echo $event_detail['status']; ?>
        </div>

        <div class="event-left">
            <img src="<?php echo $event_detail['photo_url']; ?>" alt="<?php $event_detail['group_name'] ?>photo" />
            <ul>
                <li>
                    <?php echo $event_detail['group_name']; ?>
                </li>
                <li>
                    <small><?php echo $event_detail['venue_name']; ?></small>
                </li>
                <li>
                    <small><?php echo $event_detail['phone']; ?></small>
                </li>
                <li>
                    <?php $this->fetch('eventMapButton'); ?>
                </li>
            </ul> 
        </div>

        <div class="event-right">
            <p><?php echo $event_detail['description']; ?></p>
        </div>

        <div class="event-center">
            <span><?php echo $event_detail['full_address']; ?></span>
        </div>
    </div>
</div>

<? $this->fetch('eventMap'); ?>

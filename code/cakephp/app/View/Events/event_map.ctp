<?php
/*
* Button uses HTML5 data attributes to pass name and address to JavaScript to populate modal fields
*/
?>

<?php $this->start('eventMapButton'); ?>

    <a class="btn" data-toggle="modal" href="#mapModal" data-name="<?php echo $event_detail['name']; ?>" data-address="<?php echo $event_detail['full_address']; ?>">View on Map</a>

<?php $this->end(); ?>

<?php $this->start('eventMap'); ?>

<div class="modal fade" id="map-modal" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                <h4 class="modal-title"> -- Map</h4>
            </div>
            <div class="modal-body">
                <div id="map-container">
                    <div id="map-canvas">
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                <button type="button" class="btn btn-primary">Save changes</button>
            </div>
        </div>
    </div>
</div>

<?php $this->end(); ?>


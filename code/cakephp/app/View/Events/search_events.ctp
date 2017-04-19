<div class="content-wrapper">
    <div class="content-container">
        <div class="event-center">
            <?php echo $this->Form->create('Event'); ?>

            <?php echo $form->input('user_zip', array('placeholder' => 'Enter Zip Code'); ?>
            <?php echo $this->Form->button('Submit Form', array('type' => 'submit', 'class' => 'btn')); ?>

            <?php echo $this->Form->end(); ?>
        </div>
    </div>
</div>

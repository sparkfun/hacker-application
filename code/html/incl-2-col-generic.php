<?php /* Module Name: 2 column */ ?>

<?php
$colors = [
  'mask_color' => ($bg_color === 'light-blue') ? '#239ec9' : '#FFFFFF',
  'slant_color' => ($bg_color === 'light-blue') ? '#0b2243' : '#239ec9'
]; ?>

<?php if(IS_MOBILE): ?>
  <?php include_with(
    __DIR__.'/../includes/incl-mobile-only-full-bleed-photo.php',
    array_merge(['image' => app_image_path($image, 'medium_size')], $colors)
  ); ?>
<?php endif; ?>

<div class="<?php echo $module_classes; ?> module-2-col-generic row bg-color-<?php echo $bg_color; ?>">
  <div class="small-12 small-centered columns">
    <div class="row">
      <div class="small-6 columns text-content">
        <?php if($headline): ?>
          <h3><?php echo $headline; ?></h3>
        <?php endif; ?>
        <?php if($headline2): ?>
          <?php if($use_headline_decoration): ?>
            <h2>
              <span class="heading-decor">
                <?php echo $headline2; ?>
              </span>
            </h2>
          <?php else: ?>
            <h2><?php echo $headline2; ?></h2>
          <?php endif; ?>
        <?php endif; ?>
        <?php echo $text; ?>
      </div>
      <div class="small-6 columns image-content">
        <figure>
          <?php include_with(__DIR__.'/incl-figure-slant-svg.php', $colors); ?>
          <img alt="" src="<?php echo app_image_path($image, 'medium_size'); ?>">
        </figure>
      </div>
    </div>
  </div>
</div>

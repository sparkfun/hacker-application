$(document).delegate '#update-password-page', 'pageinit', ->
  $(this).delegate '#update-password-form', 'submit', ->
    $this = $(this)
    geoverse.helpers.ajax
      url:  $this.attr('action')
      data:
        user:
          password: $('#update-password', $this).val()
          password_confirmation: $('#update-password-confirmation', $this).val()
      type: 'PUT'
      success: (data) ->
        $.mobile.changePage 'update_password_success.html'
      error: (data) ->
        errors = $('.errors', $this)
        errors.empty()
        $.each(data.errors, (index, value) -> errors.append $('<div/>').text(value))

    return false


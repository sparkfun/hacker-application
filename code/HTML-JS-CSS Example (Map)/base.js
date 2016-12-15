/** Code not for re-use. Example code only with permission from the University of Kansas **/

// Adjust base stylings/formats on page here. Certain items are format locked by site/server requirements.
jQuery( function() {

    jQuery( window ).bind( "load resize", function() {

        width = ( this.window.innerWidth > 0 ) ? this.window.innerWidth : this.screen.width;

        if( width < 768 ) {

            jQuery( 'div.sidebar-collapse' ).addClass( 'collapse' );
        } else {

            jQuery( 'div.sidebar-collapse' ).removeClass( 'collapse' );
        }
    });

	jQuery( 'ul.nav a' ).filter( function() {

			return this.href == window.location;
		}).parent().addClass( 'active' );

	jQuery( '.styled-form form button[type="submit"]' ).addClass( 'btn btn-primary' ).parent().addClass( 'text-right' );

	jQuery( '.popover-item' ).popover( { "html": true, "trigger": "hover" } );

	//Make errors prettier
	jQuery( '.input-group > ul' ).each( function() {

			jQuery( this ).addClass( 'list-group' );
			jQuery( this ).find( 'li' ).addClass( 'list-group-item list-group-item-danger' );
			jQuery( this ).insertBefore( jQuery( this ).parent() );
		});

	//Alert auto-fading
	jQuery( ".alert.fade" ).each( function() {

			if( jQuery( this ).attr( 'class' ).indexOf( 'delay-' ) >= 0 ) {

				var self = jQuery( this );
				var classes = self.attr( 'class' ).split( " " );

				for( var i = 0; i < classes.length; i++ ) {

					if( classes[i].indexOf( 'delay-' ) >= 0 ) {

						var delay = classes[i].split( '-' );

						if( delay.length == 2 ) {

							window.setTimeout(
									function() {

										if( self.hasClass( 'in' ) ) {

											if( self.hasClass( 'once' ) ) {

												self.alert( 'close' );
											} else {

												self.removeClass( 'in' );
											}
										} else {

											self.addClass( 'in' );
										}
									},
									delay[1]
								);
						}

						break;
					}
				}
			}
		});
});

function tagRequiredElements() {

	jQuery( '[required]' ).each( function () {

			jQuery( this ).parent().addClass( 'has-error' );
		});

	jQuery( '[required]' ).on( 'blur change input', function () {

			var self = jQuery( this );

			var valid = false;

			if( self.prop( 'type' ) == 'password' ) {

				self = jQuery( '[type=password][required]' );

				var result = [];

				self.each( function() {

						if( jQuery.inArray( jQuery( this ).val(), result ) < 0 ) {

							result.push( jQuery( this ).val() );
						}
					});

				if( result.length === 1 && jQuery( this ).val().length >= 6 ) {

					valid = true;

					jQuery( '#password-note' ).hide();
				} else {

					valid = false;

					jQuery( '#password-note' ).show();
					jQuery( '#password-note p:last-child' ).toggle( result.length != 1 );
				}
			} else if( self.prop( 'type' ) == 'email' ) {

				valid = ( /(.+)@(.+){2,}\.(.+){2,}/.test( self.val() ) );
			} else {

				valid = ( self.val().length > 0 );
			}

			self.parent().prev( '.required-note' ).toggle( !valid );
			self.parent().toggleClass( 'has-success', valid );
			self.parent().toggleClass( 'has-error', !valid );
		});
}
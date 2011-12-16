(function($) {
	$(document).ready(function() {
		/* Place onload functions here */
	});
	
	
	/* Place jQuery functions here */
	
		$.fn.extend({
				removeDefaultInputValue: function() {
						return this.each(function() {
								new $.RemoveDefaultInputValueMethod(this);
						});
				}
		});

    $.RemoveDefaultInputValueMethod = function(input) {
        var $input = $(input);
        var defaultValue =  $input.attr("title");
        $("form").submit(function() {
            if ($input.val() == defaultValue) {
                $input.val("");
            }
            return true;
        });
        $input.bind("focus", function(event) {
            if ($input.val() == defaultValue) {
                $input.val("");
            }
        }).bind("blur", function(event) {
            if ($input.val() == "") {
                $input.val(defaultValue);
            }
        });
    };
	
})(jQuery);

function addToQs(name, val) {
    if (val != "") {
        return name + "=" + encodeURIComponent(val) + "&";
    }
    return "";
}
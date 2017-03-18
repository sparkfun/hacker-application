module ActionView
  class PartialRenderer
    include Way
    
    def way_render_partial
      partial_content = render_partial_copy

      if show_partial_path? @view.controller
        partial_content << "#{@template.inspect}"
      end

      partial_content
    end

    alias_method :render_partial_copy, :render_partial
    alias_method :render_partial, :way_render_partial
  end
end

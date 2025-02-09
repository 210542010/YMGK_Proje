import math
import itertools

import kivy
kivy.require("2.0.0")

from kivy.app import App
from kivy.core.window import Window
from kivy.uix.relativelayout import RelativeLayout
from kivy.uix.image import Image
from kivy.uix.label import Label
from kivy.uix.button import Button
from kivy.uix.modalview import ModalView
from kivy.properties import NumericProperty, ListProperty

# For demonstration, fix a window size.
Window.size = (480, 800)

class DraggableSlice(Image):
    """
    A draggable slice widget (triangular slice).
    Each slice has a 'number' property to identify or sum with others.
    """
    number = NumericProperty(0)

    def __init__(self, **kwargs):
        super(DraggableSlice, self).__init__(**kwargs)
        self.dragging = False
        self.touch_offset_x = 0
        self.touch_offset_y = 0

    def on_touch_down(self, touch):
        # Start drag if user touches inside
        if self.collide_point(*touch.pos):
            self.dragging = True
            # Remember the offset so the slice doesn't jump
            self.touch_offset_x = self.x - touch.x
            self.touch_offset_y = self.y - touch.y
            return True
        return super(DraggableSlice, self).on_touch_down(touch)

    def on_touch_move(self, touch):
        if self.dragging:
            self.x = touch.x + self.touch_offset_x
            self.y = touch.y + self.touch_offset_y

    def on_touch_up(self, touch):
        if self.dragging:
            self.dragging = False
            return True
        return super(DraggableSlice, self).on_touch_up(touch)


class EllipseCake(Image):
    """
    Represents a merged cake (an ellipse made of 4 slices).
    'value' is the sum of the slice numbers that formed the cake.
    """
    value = NumericProperty(0)


class GameLayout(RelativeLayout):
    # Keep references to all slice widgets (the original triangles)
    slices = ListProperty([])
    # Keep references to any completed ellipse cakes
    cakes = ListProperty([])

    def __init__(self, **kwargs):
        super(GameLayout, self).__init__(**kwargs)
        self.create_slices()

        # Button to check for 4-slice merges
        self.form_cake_button = Button(
            text="Form Cake",
            size_hint=(0.3, 0.1),
            pos_hint={"x": 0.35, "y": 0.9},
            on_release=self.try_form_cakes
        )
        self.add_widget(self.form_cake_button)

        # Optional button to check if the user has formed 2 cakes
        self.check_button = Button(
            text="Check Win",
            size_hint=(0.3, 0.1),
            pos_hint={"x": 0.65, "y": 0.9},
            on_release=self.check_win_condition
        )
        self.add_widget(self.check_button)

    def create_slices(self):
        """
        Create 8 triangle slices, each with a unique integer.
        Spread them out for demonstration.
        """
        numbers = list(range(1, 9))  # 1 through 8
        for i, number in enumerate(numbers):
            # Draggable triangular slice
            slice_widget = DraggableSlice(
                source="triangle.png",  # Replace with your triangle image
                number=number,
                size_hint=(None, None),
                size=(100, 100),
                pos=(50 + 30 * i, 300)  # horizontal row
            )
            self.slices.append(slice_widget)
            self.add_widget(slice_widget)

            # Label to show the slice's number
            label = Label(
                text=str(number),
                font_size=24,
                color=(1, 1, 0, 1),  # yellow text
                center_x=slice_widget.center_x,
                center_y=slice_widget.center_y
            )
            # Keep label on top, update its position when the slice moves
            slice_widget.bind(pos=self._bind_label_position(label))
            self.add_widget(label)

    def _bind_label_position(self, label):
        """Helper: returns a function to keep label's center in sync with slice."""
        def update_label(instance, value):
            label.center_x = instance.center_x
            label.center_y = instance.center_y
        return update_label

    def try_form_cakes(self, *args):
        """
        Attempt to find any group of 4 slices that are close together.
        If found, remove them from the board and replace with a single EllipseCake.
        """
        # We'll search all combinations of 4 among the existing slices
        # and see if they form a tight group.
        slice_groups = list(itertools.combinations(self.slices, 4))

        formed_any_cake = False
        to_remove = []

        for group in slice_groups:
            if all(s in self.slices for s in group):  # Ensure they're still on the board
                if self.is_group_close(group):
                    # Summation of their numbers
                    total_value = sum(s.number for s in group)
                    # Compute the average center to position the ellipse
                    avg_x = sum(s.center_x for s in group) / 4
                    avg_y = sum(s.center_y for s in group) / 4

                    # Create an EllipseCake representing the new 4-slice cake
                    cake = EllipseCake(
                        source="ellipse.png",  # Replace with an ellipse or "full-cake" image
                        value=total_value,
                        size_hint=(None, None),
                        size=(120, 120),
                        pos=(avg_x - 60, avg_y - 60)
                    )
                    self.cakes.append(cake)
                    self.add_widget(cake)

                    # Also add a label for the sum
                    cake_label = Label(
                        text=f"{total_value}",
                        font_size=24,
                        color=(1, 0, 0, 1),
                        center_x=cake.center_x,
                        center_y=cake.center_y
                    )
                    cake.bind(pos=self._bind_label_position(cake_label))
                    self.add_widget(cake_label)

                    # Mark these slices for removal
                    to_remove.extend(group)
                    formed_any_cake = True

        # Actually remove the slices that formed a cake
        for s in to_remove:
            if s in self.slices:
                self.slices.remove(s)
                self.remove_widget(s)

        # If we formed a cake, check if we now have 2 cakes
        if formed_any_cake:
            self.check_win_condition()

    def is_group_close(self, group):
        """
        Check if the 4 slices in 'group' are close enough to form a single cake.
        We'll define 'close' by ensuring the pairwise distances between them 
        are below some threshold. You can refine this logic if you want
        a more accurate shape-fitting approach.
        """
        # Simple approach: the max distance between any two centers < some threshold
        # This threshold might need tuning depending on your UI
        threshold = 150  # experiment
        for slice_a, slice_b in itertools.combinations(group, 2):
            dist = math.dist((slice_a.center_x, slice_a.center_y),
                             (slice_b.center_x, slice_b.center_y))
            if dist > threshold:
                return False
        return True

    def check_win_condition(self, *args):
        """
        If we have exactly 2 EllipseCake widgets, the game is won.
        """
        if len(self.cakes) == 2:
            modal = ModalView(size_hint=(0.75, 0.5))
            modal.add_widget(Label(
                text="Congratulations!\nYou formed 2 complete cakes!",
                font_size=24,
                halign="center"
            ))
            modal.open()


class CakeGameApp(App):
    def build(self):
        return GameLayout()


if __name__ == "__main__":
    CakeGameApp().run()

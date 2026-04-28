<template>
  <div class="flex items-center">
    <!-- Prev -->
    <button
      class="md:p-2 p-1 bg-black/30 md:mr-6 mr-2 rounded-full hover:bg-black/50"
      @click="handlePrev"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-white" fill="none"
        viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
        <path stroke-linecap="round" stroke-linejoin="round" d="M15 19l-7-7 7-7" />
      </svg>
    </button>

    <!-- Slider -->
    <div class="w-full max-w-3xl overflow-hidden relative">
      <div
        ref="slider"
        class="flex transition-transform duration-500 ease-in-out"
        :style="{ transform: `translateX(-${currentSlide * slideWidth}px)` }"
      >
        <img
          v-for="(img, i) in images"
          :key="i"
          :src="img"
          class="w-full flex-shrink-0 rounded-2xl"
          :alt="`Slide ${i + 1}`"
        />
      </div>
    </div>

    <!-- Next -->
    <button
      class="p-1 md:p-2 bg-black/30 md:ml-6 ml-2 rounded-full hover:bg-black/50"
      @click="handleNext"
    >
      <svg xmlns="http://www.w3.org/2000/svg" class="h-6 w-6 text-white" fill="none"
        viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
        <path stroke-linecap="round" stroke-linejoin="round" d="M9 5l7 7-7 7" />
      </svg>
    </button>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted, watch } from 'vue';

const props = defineProps<{
  images: string[];
}>();

const slider = ref<HTMLDivElement | null>(null);
const currentSlide = ref(0);
const slideWidth = ref(0);
let interval: number | undefined;

const updateWidth = () => {
  if (slider.value?.children[0]) {
    slideWidth.value = (slider.value.children[0] as HTMLElement).clientWidth;
  }
};

const nextSlide = () => {
  currentSlide.value = (currentSlide.value + 1) % props.images.length;
};

const prevSlide = () => {
  currentSlide.value =
    (currentSlide.value - 1 + props.images.length) % props.images.length;
};

const resetAutoSlide = () => {
  clearInterval(interval);
  interval = setInterval(nextSlide, 3000);
};

const handleNext = () => {
  nextSlide();
  resetAutoSlide();
};

const handlePrev = () => {
  prevSlide();
  resetAutoSlide();
};

onMounted(() => {
  updateWidth();
  window.addEventListener('resize', updateWidth);
  interval = setInterval(nextSlide, 3000);
});

onUnmounted(() => {
  window.removeEventListener('resize', updateWidth);
  clearInterval(interval);
});

watch(
  () => props.images,
  () => {
    currentSlide.value = 0;
    updateWidth();
  }
);
</script>

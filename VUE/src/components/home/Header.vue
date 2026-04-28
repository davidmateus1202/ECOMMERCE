<template>
    <div class="overflow-hidden w-full min-w-screen relative mx-auto" @mouseenter="stopScroll = true"
        @mouseleave="stopScroll = false">
        <!-- left fade -->
        <div
            class="absolute left-0 top-0 h-full w-20 z-10 pointer-events-none bg-linear-to-r from-white to-transparent" />

        <!-- moving track -->
        <div class="marquee-inner flex w-fit" :style="{
            animationPlayState: stopScroll ? 'paused' : 'running',
            animationDuration: durationMs
        }">
            <div class="flex">
                <div v-for="(card, index) in loopCards" :key="index"
                    class="w-80 mx-4 h-140 relative group hover:scale-90 transition-all duration-300">
                    <img :src="card.image" alt="card" class="w-full h-full object-cover" />

                    <div class="flex items-center justify-center px-4 opacity-0 group-hover:opacity-100
                   transition-all duration-300 absolute bottom-0 backdrop-blur-md left-0
                   w-full h-full bg-black/20">
                        <p class="text-white text-lg font-semibold text-center">
                            {{ card.title }}
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <!-- right fade -->
        <div
            class="absolute right-0 top-0 h-full w-20 md:w-40 z-10 pointer-events-none bg-linear-to-l from-white to-transparent" />
    </div>
</template>

<script setup>
import { computed, ref } from "vue";

//images
import img1 from "../../assets/photos/bolso1.png";
import img2 from "../../assets/photos/bolso2.png";
import img3 from "../../assets/photos/bolso3.png";
import img4 from "../../assets/photos/bolso4.png";
import img5 from "../../assets/photos/bolso5.png";
import img6 from "../../assets/photos/bolso6.png";
import img7 from "../../assets/photos/bolso7.png";

const stopScroll = ref(false);

const cardData = [
    {
        title: "Unlock Your Creative Flow",
        image: img1,
    },
    {
        title: "Design Your Digital Future",
        image: img2,
    },
    {
        title: "Build with Passion, Ship with Pride",
        image: img3,
    },
    {
        title: "Think Big, Code Smart",
        image: img4,
    },
    {
        title: "Innovate, Iterate, Inspire",
        image: img5,
    },
    {
        title: "From Concept to Creation",
        image: img6,
    },
    {
        title: "Empowering Ideas Through Code",
        image: img7,
    },
];

// duplicate array to make continuous scroll
const loopCards = computed(() => [...cardData, ...cardData, ...cardData]);

// React had: cardData.length * 2500 + "ms"
const durationMs = computed(() => `${cardData.length * 3500}ms`);
</script>

<style scoped>
.marquee-inner {
    animation: marqueeScroll linear infinite;
}

@keyframes marqueeScroll {
    0% {
        transform: translateX(0%);
    }

    100% {
        transform: translateX(-50%);
    }
}
</style>

<template>
  <div class="min-h-screen flex items-center justify-center bg-gray-50 px-4">
    <div
      class="max-w-96 w-full text-center border border-gray-300/60 rounded-2xl px-8 bg-white shadow-md"
    >
      <h1 class="text-gray-900 text-3xl mt-10 font-medium">
        Bienvenido
      </h1>

      <!-- The form content (Login/Register) -->
      <main class="mt-5">
            <router-view />
        </main>

      <!-- Optional footer slot -->
      <div class="mt-6 mb-11">
        <slot name="footer" />
      </div>
    </div>
  </div>
</template>
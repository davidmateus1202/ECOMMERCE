<template>
    <form @submit.prevent="handleLogin">
        <p class="text-gray-500 text-sm mt-2">
        Bienvenido al Panel de Administración
      </p>
        <!-- Email -->
        <InputGroup v-model="username" type="email" placeholder="Email" required class="mt-5">
            <template #icon>
                <svg width="16" height="11" viewBox="0 0 16 11" fill="none">
                    <path fill-rule="evenodd" clip-rule="evenodd"
                        d="M0 .55.571 0H15.43l.57.55v9.9l-.571.55H.57L0 10.45zm1.143 1.138V9.9h13.714V1.69l-6.503 4.8h-.697zM13.749 1.1H2.25L8 5.356z"
                        fill="#6B7280" />
                </svg>
            </template>
        </InputGroup>

        <!-- Password -->
        <InputGroup class="mt-4" v-model="password" type="password" placeholder="Contraseña" required>
            <template #icon>
                <svg width="13" height="17" viewBox="0 0 13 17" fill="none">
                    <path
                        d="M13 8.5c0-.938-.729-1.7-1.625-1.7h-.812V4.25C10.563 1.907 8.74 0 6.5 0S2.438 1.907 2.438 4.25V6.8h-.813C.729 6.8 0 7.562 0 8.5v6.8c0 .938.729 1.7 1.625 1.7h9.75c.896 0 1.625-.762 1.625-1.7zM4.063 4.25c0-1.406 1.093-2.55 2.437-2.55s2.438 1.144 2.438 2.55V6.8H4.061z"
                        fill="#6B7280" />
                </svg>
            </template>
        </InputGroup>

        <div class="mt-5 text-left text-indigo-500">
            <RouterLink class="text-sm" :to="{ name: ROUTER_NAME.ADMIN.FORGOT_PASSWORD }">¿Olvidaste tu contraseña?</RouterLink>
        </div>

        <Button type="submit" :loading="loading" class="mt-2">
            Iniciar sesión
        </Button>
    </form>
</template>

<script setup lang="ts">
import { ref } from "vue"
import { getAuthTokenFromResponse, login, type AuthResponse } from "../../api/authApi"
import { toast } from "vue3-toastify"
import router from "../../routes"
import { ROUTER_NAME } from "../../routes/router-name"
import { Button, InputGroup } from "../../components"
import { setStoredAuthToken } from "../../service/authService"

const username = ref("")
const password = ref("")
const auth = ref<AuthResponse | null>(null)
const loading = ref(false)

const handleLogin = async () => {
    if (loading.value) return
    try {
        loading.value = true
        auth.value = await login(username.value, password.value)
        setStoredAuthToken(getAuthTokenFromResponse(auth.value))
        await router.push({ name: ROUTER_NAME.ADMIN.PRODUCTS })
    } catch (error) {
        const message = error instanceof Error ? error.message : "Error de inicio de sesión. Por favor, verifica tus credenciales e intenta nuevamente."
        toast.error(message)
        console.error("Login error:", error)
    } finally {
        loading.value = false
    }
}

</script>

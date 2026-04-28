<template>
    <form @submit.prevent="handleReset">
        <p class="text-gray-500 text-sm">
        Ingresa tu nueva contraseña para restablecer tu cuenta
      </p>
      <InputGroup class="mt-5" v-model="password" type="password" placeholder="Nueva contraseña" required>
            <template #icon>
                <svg width="13" height="17" viewBox="0 0 13 17" fill="none">
                    <path
                        d="M13 8.5c0-.938-.729-1.7-1.625-1.7h-.812V4.25C10.563 1.907 8.74 0 6.5 0S2.438 1.907 2.438 4.25V6.8h-.813C.729 6.8 0 7.562 0 8.5v6.8c0 .938.729 1.7 1.625 1.7h9.75c.896 0 1.625-.762 1.625-1.7zM4.063 4.25c0-1.406 1.093-2.55 2.437-2.55s2.438 1.144 2.438 2.55V6.8H4.061z"
                        fill="#6B7280" />
                </svg>
            </template>
        </InputGroup>

        <InputGroup class="mt-5" v-model="confirmPassword" type="password" placeholder="Confirmar contraseña" required>
            <template #icon>
                <svg width="13" height="17" viewBox="0 0 13 17" fill="none">
                    <path
                        d="M13 8.5c0-.938-.729-1.7-1.625-1.7h-.812V4.25C10.563 1.907 8.74 0 6.5 0S2.438 1.907 2.438 4.25V6.8h-.813C.729 6.8 0 7.562 0 8.5v6.8c0 .938.729 1.7 1.625 1.7h9.75c.896 0 1.625-.762 1.625-1.7zM4.063 4.25c0-1.406 1.093-2.55 2.437-2.55s2.438 1.144 2.438 2.55V6.8H4.061z"
                        fill="#6B7280" />
                </svg>
            </template>
        </InputGroup>

        <Button type="submit" :loading="loading" class="mt-5">
            Restablecer contraseña
         </Button>
    </form>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { ROUTER_NAME } from '../../routes/router-name';
import { resetPassword } from "../../api/authApi";
import { toast } from 'vue3-toastify';

// components
import { InputGroup, Button } from '../../components';

// router
const route = useRoute();
const router = useRouter();

// state
const password = ref('');
const confirmPassword = ref('');
const token = ref('');
const loading = ref(false);

onMounted(() => {
    token.value = route.query.token as string || '';
})

const handleReset = async () => {
    if (password.value !== confirmPassword.value) {
        toast.error("Las contraseñas no coinciden");
        return;
    }

    loading.value = true;
    const response = await resetPassword(token.value, password.value);

    if (response.status === 200) {
        toast.success("Contraseña restablecida exitosamente");
        router.push({ name: ROUTER_NAME.ADMIN.LOGIN });
    } else {
        toast.error("Hubo un error al intentar restablecer la contraseña");
    }

    loading.value = false;
}


</script>
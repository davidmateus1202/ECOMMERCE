<template>
    <form @submit.prevent="handleForget">
        <p class="text-gray-500 text-sm">
            Ingresa tu correo electrónico para restablecer tu contraseña
        </p>
        <InputGroup class="mt-5" v-model="email" type="email" placeholder="Email" required>
            <template #icon>
                <svg width="16" height="11" viewBox="0 0 16 11" fill="none">
                    <path fill-rule="evenodd" clip-rule="evenodd"
                        d="M0 .55.571 0H15.43l.57.55v9.9l-.571.55H.57L0 10.45zm1.143 1.138V9.9h13.714V1.69l-6.503 4.8h-.697zM13.749 1.1H2.25L8 5.356z"
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
import { ref } from 'vue';
import { forgotPassword } from "../../api/authApi";
import { toast } from 'vue3-toastify';

// components
import { InputGroup, Button } from '../../components';

// state
const email = ref('');
const loading = ref(false);

// function to handle password reset
const handleForget = async () => {
    loading.value = true;
    const response = await forgotPassword(email.value);

    if (response.status === 200) {
        toast.success("Se ha enviado un correo para restablecer la contraseña");
    } else {
        toast.error("Hubo un error al intentar restablecer la contraseña");
    }

    loading.value = false;
}

</script>
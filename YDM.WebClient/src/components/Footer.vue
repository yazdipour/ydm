<template>
  <v-footer
    dark
    height="auto">
    <v-card
      class="flex"
      flat
      tile>
      <v-card-title class="teal">
        <strong class="subheading">Get connected with us on social networks!</strong>
    <a href="https://github.com/yazdipour/ydm">Github/yazdipour/YDM</a>
        <v-spacer></v-spacer>
        <v-btn
          v-for="icon in icons"
          :key="icon"
          class="mx-3"
          dark
          icon>
          <v-icon size="24px">{{ icon }}</v-icon>
        </v-btn>
      </v-card-title>

      <v-card-actions class="grey darken-3 justify-center">
            <div>&copy; {{ new Date().getFullYear() }}<strong>YDM</strong></div>
      </v-card-actions>
    </v-card>
  </v-footer>
</template>
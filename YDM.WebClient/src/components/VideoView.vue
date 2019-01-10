<template v-if="video && video.length">
 <v-layout ma-1 xs12 md7 align-center justify-center>
  <v-card class="white--text">
    <v-layout>
      <v-flex>
        <v-img :src="video.Image" height="125"/>
      </v-flex>
      <v-flex xs7>
        <v-container>
          <v-card-title style="padding-left:0">
          <div style="width:100%;" >
            <div class="headline" id="title">{{video.Title}}</div>
            <div>{{convertDuration(video.Duration)}}</div>
            <a :href="video.id">{{video.Id}}</a>
          </div>
        </v-card-title>
        </v-container>
      </v-flex>
    </v-layout>
    <v-divider light/>
    <v-btn small v-for="(item, index) in links" :key="index" @click="openUrl(item)">
    {{item.quality}}</v-btn>
  </v-card>
</v-layout>
</template>

<script>
export default {
  props:{
    video:Object,
    links:[]
  },
  methods: {
    openUrl(item){
      window.open(item.url, '_blank');
    },
    convertDuration(inSecond){
      if(inSecond<60) return inSecond;
       var sec_num = parseInt(inSecond, 10); // don't forget the second param
        var hours   = Math.floor(sec_num / 3600);
        var minutes = Math.floor((sec_num - (hours * 3600)) / 60);
        var seconds = sec_num - (hours * 3600) - (minutes * 60);

        if (hours   < 10) {hours   = "0"+hours;}
        if (minutes < 10) {minutes = "0"+minutes;}
        if (seconds < 10) {seconds = "0"+seconds;}
        return hours+':'+minutes+':'+seconds;
    }
  },
}
</script>

<style scoped>
a {width:100px}
#title{
  white-space:nowrap;
  text-overflow:ellipsis;
  overflow:hidden;
}
</style>
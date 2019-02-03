<template v-if="video && video.length">
 <v-layout ma-2 xs12 md7 align-center justify-center>
  <v-card class="white--text" style="width:100%">
    <v-layout>
      <v-flex>
        <v-img :src="video.Image" max-height="160"/>
      </v-flex>
      <v-flex xs7>
        <v-container>
          <v-card-title style="padding:0">
          <div style="width:100%;" >
            <div class="headline" id="title">{{video.Title}}</div>
            <div v-if="video.Duration">{{convertDuration(video.Duration)}}</div>
            <div v-else>{{video.Channel}}</div>
            <a v-if="links[0]" :href="video.id">{{video.Id}}</a>
            <v-btn v-else @click="redirectToSelf(video.Id)" color="primary" style="margin-left: 0;" dark>Get The Video</v-btn>
          </div>
        </v-card-title>
        </v-container>
      </v-flex>
    </v-layout>
    <div v-if="links[0]" >
      <v-divider light/>
      <span class="ma-2">Direct Download (will expire after a day):</span>
      <v-btn small v-for="(item, index) in links" :key="index" @click="openUrl(item)">{{item.quality}}</v-btn>
    </div>
    <div v-if="links[0]" >
      <v-divider light/>
      <span class="ma-2">Reusable Links:</span>
      <v-btn small color="grey darken-2" v-for="(item, index) in links" :key="index" @click="openReusableUrl(item)">{{item.quality}}</v-btn>
    </div>
  </v-card>
</v-layout>
</template>

<script>
export default {
  props:{
    video:Object,
    links:Object
  },
  methods: {
    openUrl(item){
      window.open(item.url, '_blank');
    },
    openReusableUrl(item){
      window.open('http://ydm.herokuapp.com/getvideo.php?i='+this.video.Id+'&tag='+item.tag, '_blank');
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
    },
    redirectToSelf(videoId){
      var url=window.location.href;
      url = url.substring(0,url.lastIndexOf('?v='));
      window.open(url+'?v='+videoId,'_blank');
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
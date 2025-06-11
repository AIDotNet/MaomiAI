// 定义事件

export const INCREMENT = 'INCREMENT';
export const DECREMENT = 'DECREMENT';
export const SET_USERNAME = 'SET_USERNAME';
export const SET_TEAM_ID = 'SET_TEAM_ID';
export const SET_CURRENT_TEAM = 'SET_CURRENT_TEAM';

export const increment = () => ({
  type: INCREMENT,
});

export const decrement = () => ({
  type: DECREMENT,
});

export const setUsername = (username: string) => ({
  type: SET_USERNAME,
  payload: username,
});

export const setTeamId = (teamId: string) => ({
  type: SET_TEAM_ID,
  payload: teamId,
});

export const setCurrentTeam = (team: any) => ({
  type: SET_CURRENT_TEAM,
  payload: team,
});
